using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Dashboard.Models;
using Northwind.Models;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Dashboard.Endpoints;

[Authorize]
public class RenderDashboard : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<RenderedDashboardResult>
{
    private readonly DashboardRuntimeService _runtime;
    private readonly IHttpContextAccessor _http;
    private readonly NorthwindContext _db; // om du hämtar config från DB

    public RenderDashboard(DashboardRuntimeService runtime, IHttpContextAccessor http, NorthwindContext db)
    {
        _runtime = runtime;
        _http = http;
        _db = db;
    }

    [HttpGet("api/dashboard/render")]
    public override async Task<ActionResult<RenderedDashboardResult>> HandleAsync(CancellationToken ct = default)
    {
        var user = _http.HttpContext!.User;
        var role = user.FindFirstValue(ClaimTypes.Role);
        var supplierId = user.FindFirstValue("SupplierId");

        // välj config dynamiskt beroende på användare
        IQueryable<DashboardConfig> query = _db.DashboardConfigs;

        if (string.Equals(role, "Supplier", StringComparison.OrdinalIgnoreCase) && supplierId != null)
            query = query.Where(c => c.SupplierId == int.Parse(supplierId));

        else if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            query = query.Where(c => c.Key == "admin-dashboard");

        else
            query = query.Where(c => c.Key == "default");

        var cfg = await query.OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
                             .Select(c => c.ConfigJson)
                             .FirstOrDefaultAsync(ct);

        if (cfg is null)
            return NotFound("Ingen dashboardkonfiguration hittades.");

        var result = await _runtime.RenderAsync(cfg, user, ct);
        return Ok(result);
    }
}
