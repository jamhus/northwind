using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Dashboard.Handlers;

public class TotalSalesHandler : BaseHandler
{
    private readonly IHttpContextAccessor _http;
    public TotalSalesHandler(NorthwindContext db, IHttpContextAccessor http) : base(db)
    {
        _http = http;
    }

    public override string Type => "TotalSales";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var rolesValue = store.Get("userRoles")?.ToString() ?? string.Empty;
        var roles = rolesValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                              .Select(r => r.ToLowerInvariant())
                              .ToHashSet();

        // Hämta supplierId (om finns)
        int.TryParse(store.Get("supplierId")?.ToString(), out var supplierId);
        int.TryParse(store.Get("employeeId")?.ToString(), out var employeeId);

        bool isSupplier = supplierId > 0 && roles.Contains("supplier");
        bool isEmployee = employeeId > 0 && roles.Contains("employee");


        var query = Db.OrderDetails
            .Include(d => d.Order)
            .Include(d => d.Product)
            .AsQueryable();

        if (isSupplier)
        {
            query = query.Where(d => d.Product.SupplierId == supplierId);
        }
        else if (isEmployee)
        {
            query = query.Where(d => d.Order.EmployeeId == employeeId);
        }

        var totalSales = await query
            .SumAsync(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount), ct);

        return Math.Round(totalSales, 1);
    }
}
