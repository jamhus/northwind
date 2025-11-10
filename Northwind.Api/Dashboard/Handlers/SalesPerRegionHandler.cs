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

        var data = await Db.Orders
            .Where(o => o.ShipCountry != null)
            .Select(o => new
            {
                Region = o.ShipCountry!,
                Total = o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .AsNoTracking()
            .Take(top)
            .ToListAsync(ct);

        var result = data.GroupBy(x => x.Region)
                         .Select(g => new { region = g.Key, totalSales = Math.Round(g.Sum(x => x.Total),1) })
                         .OrderByDescending(x => x.totalSales)
                         .ToList();

        return result;
    }
}
