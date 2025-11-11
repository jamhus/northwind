using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Auth;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Endpoints.Dashboard;

[Authorize(Roles = Roles.Admin)]

public class UpdateDashboardConfig : EndpointBaseAsync
    .WithRequest<DashboardConfigRequest>
    .WithActionResult
{
    private readonly NorthwindContext _db;

    public UpdateDashboardConfig(NorthwindContext db)
    {
        _db = db;
    }

    [HttpPut("api/dashboardConfig")]
    public override async Task<ActionResult> HandleAsync(
        DashboardConfigRequest request,
        CancellationToken ct = default)
    {
        var entity = new DashboardConfig
        {
            CompanyId = request.CompanyId,
            ConfigJson = request.Json,
            CreatedAt = DateTime.UtcNow
        };

        _db.DashboardConfigs.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Ok();
    }
}

public class DashboardConfigRequest
{
    public int CompanyId { get; set; }
    public string Json { get; set; } = string.Empty;
}
