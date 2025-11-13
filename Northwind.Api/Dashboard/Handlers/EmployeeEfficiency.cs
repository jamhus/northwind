using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class EmployeeEfficiencyHandler : BaseHandler
{
    public EmployeeEfficiencyHandler(NorthwindContext db) : base(db) { }

    public override string Type => "EmployeeEfficiency";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var rolesValue = store.Get("userRoles")?.ToString() ?? string.Empty;
        var roles = rolesValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                              .Select(r => r.ToLowerInvariant())
                              .ToHashSet();

        int.TryParse(store.Get("employeeId")?.ToString(), out var employeeId);

        var query = Db.Orders
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
            .AsQueryable();

        // 🔹 Employee → bara sina egna siffror
        if (roles.Contains("employee") && employeeId > 0)
        {
            query = query.Where(o => o.EmployeeId == employeeId);
        }

        // 🔹 Supplier → ej relevant
        else if (roles.Contains("supplier"))
        {
            return Array.Empty<object>();
        }

        // 🔹 Manager/Admin → ser alla
        // (ingen filtrering här)

        var grouped = await query
            .Where(o => o.Employee != null && o.OrderDetails.Any())
            .GroupBy(o => new
            {
                o.Employee!.EmployeeId,
                EmployeeName = o.Employee.FirstName + " " + o.Employee.LastName
            })
            .Select(g => new
            {
                Employee = g.Key.EmployeeName,
                TotalSales = g.Sum(o =>
                    o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))),
                TotalOrders = g.Count()
            })
            .AsNoTracking()
            .ToListAsync(ct);

        // 🔹 Om Employee — returnera bara sin egen rad
        if (roles.Contains("employee") && employeeId > 0)
        {
            grouped = grouped.Where(x =>
                Db.Employees.Any(e => e.EmployeeId == employeeId && (x.Employee == (e.FirstName + " " + e.LastName)))
            ).ToList();
        }

        var result = grouped
            .Select(x => new
            {
                employee = x.Employee,
                efficiency = x.TotalOrders > 0 ? Math.Round(x.TotalSales / x.TotalOrders, 2) : 0
            })
            .OrderByDescending(x => x.efficiency)
            .ToList();

        return result;
    }
}
