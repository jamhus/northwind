using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class EmployeeWorkloadHandler : BaseHandler
{
    public EmployeeWorkloadHandler(NorthwindContext db) : base(db) { }

    public override string Type => "EmployeeWorkload";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var roles = ((store.Get("userRoles")?.ToString()) ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(r => r.ToLowerInvariant())
            .ToHashSet();

        int.TryParse(store.Get("employeeId")?.ToString(), out var employeeId);

        var query = Db.Orders.Include(o => o.Employee).AsQueryable();

        if (roles.Contains("employee") && employeeId > 0)
            query = query.Where(o => o.EmployeeId == employeeId);
        else if (roles.Contains("supplier"))
            return Array.Empty<object>();

        var data = await query
            .Where(o => o.Employee != null)
            .GroupBy(o => o.Employee!.FirstName + " " + o.Employee.LastName)
            .Select(g => new
            {
                employee = g.Key,
                orderCount = g.Count()
            })
            .OrderByDescending(x => x.orderCount)
            .AsNoTracking()
            .ToListAsync(ct);

        return data;
    }
}
