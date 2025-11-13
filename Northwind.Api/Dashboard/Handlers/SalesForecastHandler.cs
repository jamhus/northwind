using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class SalesForecastHandler : BaseHandler
{
    public SalesForecastHandler(NorthwindContext db) : base(db) { }

    public override string Type => "SalesForecast";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var oneYearAgo = Db.Orders.Min(o => o.OrderDate)!;

        var roles = ((store.Get("userRoles")?.ToString()) ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(r => r.ToLowerInvariant())
            .ToHashSet();

        int.TryParse(store.Get("supplierId")?.ToString(), out var supplierId);

        var query = Db.OrderDetails
            .Include(d => d.Order)
            .Include(d => d.Product)
            .Where(d => d.Order.OrderDate >= oneYearAgo)
            .AsQueryable();

        if (roles.Contains("supplier") && supplierId > 0)
            query = query.Where(d => d.Product.SupplierId == supplierId);

        var monthlyRaw = await query
        .GroupBy(d => new { d.Order.OrderDate!.Value.Year, d.Order.OrderDate!.Value.Month })
        .Select(g => new
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            Total = g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount))
        })
        .OrderBy(g => g.Year)
        .ThenBy(g => g.Month)
        .AsNoTracking()
        .ToListAsync(ct);

        var monthly = monthlyRaw
            .Select(x => new
            {
                month = new DateTime(x.Year, x.Month, 1),
                total = x.Total
            })
            .ToList();


        if (monthly.Count < 2)
            return monthly;

        var growth = (monthly.Last().total - monthly.First().total) / monthly.First().total;
        var nextMonth = monthly.Last().month.AddMonths(1);

        var forecast = monthly.Concat(new[]
        {
            new { month = nextMonth, total = monthly.Last().total * (1 + growth) }
        }).ToList();

        return forecast.Select(x => new
        {
            label = x.month.ToString("MMM yyyy"),
            value = Math.Round(x.total, 1)
        });
    }
}
