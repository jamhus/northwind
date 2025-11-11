using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class SalesPerRegionHandler : BaseHandler
{
    public SalesPerRegionHandler(NorthwindContext db) : base(db) { }
    public override string Type => "SalesPerRegion";

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var top = GetTopArg(settings, 6);

        var query = FilterOrders(
            Db.Orders.Include(o => o.OrderDetails),
            store
        );

        var result = await query
            .Where(o => o.ShipCountry != null)
            .GroupBy(o => o.ShipCountry!)
            .Select(g => new
            {
                Region = g.Key,
                TotalSales = g.SelectMany(o => o.OrderDetails)
                              .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(top)
            .AsNoTracking()
            .ToListAsync(ct);

        return result.Select(x => new
        {
            region = x.Region ?? "Okänd region",
            totalSales = Math.Round(x.TotalSales, 1)
        });
    }
}
