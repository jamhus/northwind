using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class OutlierProductsHandler : BaseHandler
{
    public OutlierProductsHandler(NorthwindContext db) : base(db) { }

    public override string Type => "OutlierProducts";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var roles = ((store.Get("userRoles")?.ToString()) ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(r => r.ToLowerInvariant())
            .ToHashSet();

        int.TryParse(store.Get("supplierId")?.ToString(), out var supplierId);

        var query = Db.OrderDetails
            .Include(d => d.Product)
            .AsQueryable();

        if (roles.Contains("supplier") && supplierId > 0)
            query = query.Where(d => d.Product.SupplierId == supplierId);

        var sales = await query
            .GroupBy(d => d.Product.ProductName)
            .Select(g => new
            {
                product = g.Key,
                total = g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount))
            })
            .AsNoTracking()
            .ToListAsync(ct);

        var avg = sales.Average(x => x.total);
        var stdDev = Math.Sqrt(sales.Sum(x => Math.Pow((double)(x.total - avg), 2)) / sales.Count);

        var outliers = sales
            .Where(x => Math.Abs((double)(x.total - avg)) > stdDev * 1.5)
            .OrderByDescending(x => x.total)
            .ToList();

        return outliers;
    }
}
