using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class DiscountEfficiencyHandler : BaseHandler
{
    public DiscountEfficiencyHandler(NorthwindContext db) : base(db) { }

    public override string Type => "DiscountEfficiency";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        // Roller (men de påverkar inte rabattanalysen mycket)
        var roles = ((store.Get("userRoles")?.ToString()) ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(r => r.ToLowerInvariant())
            .ToHashSet();

        int.TryParse(store.Get("supplierId")?.ToString(), out var supplierId);

        var query = Db.OrderDetails
            .Include(d => d.Product)
            .Include(d => d.Order)
            .Where(d => d.Order.OrderDate != null)
            .AsQueryable();

        if (roles.Contains("supplier") && supplierId > 0)
            query = query.Where(d => d.Product.SupplierId == supplierId);

        // Grupp per rabattnivå
        var result = await query
            .GroupBy(d => d.Discount)
            .Select(g => new
            {
                discount = g.Key,
                avgOrderValue = g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount)) / g.Count(),
                totalOrders = g.Count()
            })
            .OrderBy(g => g.discount)
            .AsNoTracking()
            .ToListAsync(ct);

        return result.Select(x => new
        {
            discount = $"{x.discount:P0}",
            avgOrderValue = Math.Round(x.avgOrderValue, 2),
            orderCount = x.totalOrders
        });
    }
}
