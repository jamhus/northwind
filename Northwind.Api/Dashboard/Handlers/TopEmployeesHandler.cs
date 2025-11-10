using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TopEmployeesHandler : BaseHandler
{
    public TopEmployeesHandler(NorthwindContext db) : base(db) { }
    public override string Type => "TopEmployees";

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var top = GetTopArg(settings, 6);

        var data = await Db.Orders
            .Where(o => o.Employee != null)
            .GroupBy(o => o.Employee!.FirstName + " " + o.Employee!.LastName)
            .Select(g => new
            {
                employeeName = g.Key,
                totalSales = Math.Round(g.SelectMany(o => o.OrderDetails)
                              .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount)),1)
            })
            .OrderByDescending(x => x.totalSales)
            .Take(top)
            .AsNoTracking()
            .ToListAsync(ct);

        return data;
    }
}
