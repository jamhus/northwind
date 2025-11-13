using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Endpoints.Dashboard;

public class GetDashboardConfig : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult<object>
{
    private readonly NorthwindContext _db;
    private readonly IWebHostEnvironment _env;

    public GetDashboardConfig(NorthwindContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [HttpGet("api/dashboardConfig/{companyId}", Name = nameof(GetDashboardConfig))]
    public override async Task<ActionResult<object>> HandleAsync(
        int companyId,
        CancellationToken ct = default)
    {

        // 🔹 1. Försök hitta en config i databasen för det företaget
        var config = await _db.DashboardConfigs
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(ct);

        if (config != null)
            return Ok(config.ConfigJson);

        // 🔹 2. Fanns inget – ladda compactDashboard.json från filsystemet
        var path = Path.Combine(_env.ContentRootPath, "Structures", "compactDashboard.json");

        if (System.IO.File.Exists(path))
        {
            var defaultJson = await System.IO.File.ReadAllTextAsync(path, ct);

            return Ok(defaultJson);
        }

        // 🔹 4. Om inget hittas alls
        return NotFound(new { message = "No dashboard config found or default file missing." });
    }
}
