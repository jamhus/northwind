using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class SupplierTotalSalesHandler : BaseHandler
{
    public SupplierTotalSalesHandler(NorthwindContext db) : base(db) { }

    public override string Type => "SupplierTotalSales";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        // Hämta roller från ParameterStore (kommer från DynamicData.getCurrentRoles)
        var rolesValue = store.Get("userRoles")?.ToString() ?? string.Empty;
        var roles = rolesValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                              .Select(r => r.ToLowerInvariant())
                              .ToHashSet();

        // Hämta supplierId (om finns)
        int.TryParse(store.Get("supplierId")?.ToString(), out var supplierId);
        bool isSupplier = supplierId > 0 && roles.Contains("supplier");

        var query = Db.OrderDetails
            .Include(od => od.Product)
            .ThenInclude(p => p.Supplier)
            .Include(od => od.Order)
            .Where(od => od.Product != null && od.Order != null)
            .AsQueryable();

        if (isSupplier)
        {
            // 🔹 Supplier: visa bara egna produkter
            query = query.Where(od => od.Product!.SupplierId == supplierId);
        }

        // 🔹 Grupp efter supplier
        var grouped = await query
            .GroupBy(od => od.Product!.Supplier!.CompanyName)
            .Select(g => new
            {
                Supplier = g.Key!,
                TotalSales = Math.Round(
                    g.Sum(x => x.UnitPrice * x.Quantity * (decimal)(1 - x.Discount)), 1)
            })
            .OrderByDescending(x => x.TotalSales)
            .ToListAsync(ct);

        if (isSupplier)
        {
            // Returnera bara supplierns egen rad
            return grouped.FirstOrDefault() ?? new { Supplier = "(Ingen data)", TotalSales = 0m };
        }

        // 🔹 Manager/Admin ser alla suppliers
        return grouped;
    }
}
