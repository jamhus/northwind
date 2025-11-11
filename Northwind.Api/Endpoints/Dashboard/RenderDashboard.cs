using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Dashboard.Models;
using Northwind.Models;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Dashboard;

public class RenderDashboard : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<RenderedDashboardResult>
{
    private readonly DashboardRuntimeService _runtime;
    private readonly IHttpContextAccessor _http;
    private readonly NorthwindContext _db;
    private readonly IWebHostEnvironment _env;

    public RenderDashboard(
        DashboardRuntimeService runtime,
        IHttpContextAccessor http,
        NorthwindContext db,
        IWebHostEnvironment env)
    {
        _runtime = runtime;
        _http = http;
        _db = db;
        _env = env;
    }

    [HttpGet("api/dashboard/render")]
    public override async Task<ActionResult<RenderedDashboardResult>> HandleAsync(CancellationToken ct = default)
    {
        var user = _http.HttpContext!.User;
        var role = user.FindFirstValue(ClaimTypes.Role);
        var supplierId = user.FindFirstValue("SupplierId");

        IQueryable<DashboardConfig> query = _db.DashboardConfigs.AsNoTracking();

        // 🔹 1. Välj config beroende på roll/supplier
        if (string.Equals(role, "Supplier", StringComparison.OrdinalIgnoreCase) && supplierId != null)
        {
            query = query.Where(c => c.CompanyId == int.Parse(supplierId));
        }
        else
        {
            query = query.Where(c => c.Key == "default");
        }

        var cfg = await query
            .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
            .Select(c => c.ConfigJson)
            .FirstOrDefaultAsync(ct);

        // 🔹 2. Om inget hittas, försök ladda defaultDashboard.json
        if (cfg is null)
        {
            var path = Path.Combine(_env.ContentRootPath, "Structures", "defaultDashboard.json");
            if (System.IO.File.Exists(path))
            {
                cfg = await System.IO.File.ReadAllTextAsync(path, ct);

                _db.DashboardConfigs.Add(new DashboardConfig
                {
                    Key = "default",
                    CompanyId = supplierId != null ? int.Parse(supplierId) : null,
                    ConfigJson = cfg,
                    CreatedAt = DateTime.UtcNow
                });

                await _db.SaveChangesAsync(ct);
            }

        }

        // 🔹 3. Rendera dashboard med vald konfiguration
        var result = await _runtime.RenderAsync(cfg, user, ct);
        return Ok(result);
    }
}
