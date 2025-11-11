using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class SalesPerMonthHandler : BaseHandler
{
    public override string Type => "SalesPerMonth";
    public SalesPerMonthHandler(NorthwindContext db) : base(db) { }

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore ps, CancellationToken ct)
    {
        var query = FilterOrders(
            Db.Orders
                .Include(o => o.OrderDetails),
            ps
        );

        var grouped = await query
            .Where(o => o.OrderDate != null)
            .GroupBy(o => new { o.OrderDate!.Value.Year, o.OrderDate!.Value.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalSales = g.SelectMany(o => o.OrderDetails)
                              .Sum(od => od.Quantity * od.UnitPrice)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync(ct);

        return grouped.Select(x => new
        {
            x.Year,
            x.Month,
            TotalSales = Round(x.TotalSales, 1)
        });
    }
}
