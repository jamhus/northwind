using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class SalesByMonthHandler : BaseHandler
{
    public SalesByMonthHandler(NorthwindContext db) : base(db) { }
    public override string Type => "SalesPerMonth"; // parameterKey: "salesPerMonth" i din JSON

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var data = await Db.Orders
            .Where(o => o.OrderDate != null)
            .OrderBy(o => o.OrderDate)
            .Select(o => new
            {
                Month = $"{o.OrderDate!.Value:yyyy-MM}",
                Total = Math.Round(o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount)),1)
            })
            .AsNoTracking()
            .ToListAsync(ct);

        var result = data.GroupBy(x => x.Month)
                         .Select(g => new { month = g.Key, totalSales = g.Sum(x => x.Total) })
                         .OrderBy(x => x.month)
                         .ToList();

        return result;
    }
}
