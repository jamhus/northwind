using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class DiscountByCategoryHandler : BaseHandler
{
    public DiscountByCategoryHandler(NorthwindContext db) : base(db) { }

    public override string Type => "DiscountByCategory";

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
            .ThenInclude(p => p.Category)
            .Include(d => d.Order)
            .Where(d => d.Order.OrderDate != null)
            .AsQueryable();

        if (roles.Contains("supplier") && supplierId > 0)
            query = query.Where(d => d.Product.SupplierId == supplierId);

        var data = await query
            .GroupBy(d => new { d.Discount, d.Product.Category!.CategoryName })
            .Select(g => new
            {
                discount = g.Key.Discount,
                category = g.Key.CategoryName,
                totalSales = g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount))
            })
            .OrderBy(x => x.discount)
            .AsNoTracking()
            .ToListAsync(ct);

        // konvertera till enklare struktur för frontend
        return data.Select(x => new
        {
            discount = $"{x.discount:P0}",
            category = x.category,
            totalSales = Math.Round(x.totalSales, 2)
        }).ToList();
    }
}
