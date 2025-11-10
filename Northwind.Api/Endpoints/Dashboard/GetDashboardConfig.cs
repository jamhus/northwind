using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Endpoints.Dashboard;

public class GetDashboardConfig : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<object>
{
    private readonly NorthwindContext _db;
    private readonly IWebHostEnvironment _env;

    public GetDashboardConfig(NorthwindContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [HttpGet("api/dashboardConfig")]
    public override async Task<ActionResult<object>> HandleAsync(CancellationToken ct = default)
    {
        var config = await _db.DashboardConfigs.AsNoTracking()
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(ct);

        if (config != null)
            return Ok(config.ConfigJson);

        var path = Path.Combine(_env.ContentRootPath, "Structures", "defaultDashboard.json");

        if (System.IO.File.Exists(path))
        {
            var defaultJson = await System.IO.File.ReadAllTextAsync(path, ct);

            // valfritt: spara i databasen direkt
            _db.DashboardConfigs.Add(new DashboardConfig
            {
                CompanyId = 1,
                ConfigJson = defaultJson,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync(ct);

            return Ok(defaultJson);
        }

        return Ok(new { message = "No dashboard config found, and no default file available." });
    }
}
