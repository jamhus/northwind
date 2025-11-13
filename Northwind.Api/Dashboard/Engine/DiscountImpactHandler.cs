using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class DiscountImpactHandler : BaseHandler
{
    public DiscountImpactHandler(NorthwindContext db) : base(db) { }

    public override string Type => "DiscountImpact";

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
        int.TryParse(store.Get("employeeId")?.ToString(), out var employeeId);

        var query = Db.OrderDetails
            .Include(d => d.Order)
            .Include(d => d.Product)
            .AsQueryable();

        if (roles.Contains("supplier") && supplierId > 0)
            query = query.Where(d => d.Product.SupplierId == supplierId);
        else if (roles.Contains("employee") && employeeId > 0)
            query = query.Where(d => d.Order.EmployeeId == employeeId);

        var data = await query
            .GroupBy(d => d.Discount)
            .Select(g => new
            {
                discount = Math.Round(g.Key * 100,1),
                totalSales = Math.Round(g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount)),1)
            })
            .OrderByDescending(x => x.discount)
            .AsNoTracking()
            .ToListAsync(ct);

        return data;
    }
}
