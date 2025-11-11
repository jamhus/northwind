using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TopProductsHandler : BaseHandler
{
    public override string Type => "TopProducts";
    public TopProductsHandler(NorthwindContext db) : base(db) { }

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore ps, CancellationToken ct)
    {
        var top = GetTopArg(settings, 5);

        var query = FilterOrders(
            Db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product),
            ps
        );

        var topProducts = await query
            .SelectMany(o => o.OrderDetails)
            .Where(od => od.Product != null)
            .GroupBy(od => od.Product.ProductName)
            .Select(g => new
            {
                Product = g.Key,
                TotalSales = g.Sum(od => od.Quantity * od.UnitPrice)
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(top)
            .ToListAsync(ct);

        return topProducts.Select(x => new
        {
            x.Product,
            TotalSales = Round(x.TotalSales, 1)
        });
    }
}
