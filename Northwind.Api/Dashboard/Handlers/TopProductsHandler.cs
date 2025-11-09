using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TopProductsHandler : BaseHandler
{
    public TopProductsHandler(NorthwindContext db) : base(db) { }
    public override string Type => "TopProducts";

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var top = GetTopArg(settings, 5);

        var data = await Db.OrderDetails
            .GroupBy(d => d.Product!.ProductName)
            .Select(g => new
            {
                name = g.Key!,
                totalSales = g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount))
            })
            .OrderByDescending(x => x.totalSales)
            .Take(top)
            .AsNoTracking()
            .ToListAsync(ct);

        return data;
    }
}
