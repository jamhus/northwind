using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Dashboard.Handlers;

public class ProductLifecycleHandler : BaseHandler
{
    private readonly IHttpContextAccessor _http;

    public ProductLifecycleHandler(NorthwindContext db, IHttpContextAccessor http) : base(db)
    {
        _http = http;
    }

    public override string Type => "ProductLifecycle";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var user = _http.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role);
        var supplierId = user?.FindFirstValue("SupplierId");

        var take = GetTakeValue(settings, 5); // default top 5 produkter

        var query = Db.OrderDetails
            .Include(d => d.Order)
            .Include(d => d.Product)
            .Where(d => d.Order.OrderDate != null)
            .AsQueryable();

        // 🔹 Supplier ser endast sina produkter
        if (role == "Supplier" && int.TryParse(supplierId, out var sid))
            query = query.Where(d => d.Product.SupplierId == sid);

        // 🔹 Hämta topp-produkter (baserat på total försäljning)
        var topProductIds = await query
            .GroupBy(d => new { d.ProductId, d.Product.ProductName })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.ProductName,
                TotalSales = g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount))
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(take)
            .Select(x => x.ProductId)
            .ToListAsync(ct);

        if (!topProductIds.Any())
            return Array.Empty<object>();

        // 🔹 Sammanställ månadsvis försäljning per produkt
        var sales = await Db.OrderDetails
            .Include(d => d.Order)
            .Where(d => topProductIds.Contains(d.ProductId) && d.Order.OrderDate != null)
            .GroupBy(d => new
            {
                d.Product.ProductName,
                Year = d.Order.OrderDate!.Value.Year,
                Month = d.Order.OrderDate!.Value.Month
            })
            .Select(g => new
            {
                Product = g.Key.ProductName,
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalSales = g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount))
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .AsNoTracking()
            .ToListAsync(ct);

        // 🔹 Strukturera för frontend (gruppera per produkt)
        var grouped = sales
            .GroupBy(x => x.Product)
            .Select(g => new
            {
                product = g.Key,
                data = g.Select(x => new
                {
                    month = $"{x.Year}-{x.Month:D2}",
                    totalSales = Math.Round(x.TotalSales, 1)
                })
            })
            .ToList();

        return grouped;
    }
}
