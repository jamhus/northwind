using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TopEmployeesHandler : BaseHandler
{
    public override string Type => "TopEmployees";
    public TopEmployeesHandler(NorthwindContext db) : base(db) { }

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore ps, CancellationToken ct)
    {
        var top = GetTakeValue(settings, 6);

        var query = FilterOrders(
            Db.Orders
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails),
            ps
        );

        var topEmployees = await query
            .Where(o => o.Employee != null)
            .GroupBy(o => new { o.Employee.FirstName, o.Employee.LastName })
            .Select(g => new
            {
                EmployeeName = g.Key.FirstName + " " + g.Key.LastName,
                TotalSales = g.SelectMany(o => o.OrderDetails)
                              .Sum(od => od.Quantity * od.UnitPrice)
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(top)
            .ToListAsync(ct);

        return topEmployees.Select(x => new
        {
            x.EmployeeName,
            TotalSales = Round(x.TotalSales, 1)
        });
    }
}
