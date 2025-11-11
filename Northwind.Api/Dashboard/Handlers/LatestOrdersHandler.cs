using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Dashboard.Handlers;

public class LatestOrdersHandler : BaseHandler
{
    private readonly IHttpContextAccessor _http;

    public LatestOrdersHandler(NorthwindContext db, IHttpContextAccessor http) : base(db)
    {
        _http = http;
    }

    public override string Type => "LatestOrders";

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var user = _http.HttpContext?.User;
        var top = GetTopArg(settings, 5);

        if (user == null || !user.Identity?.IsAuthenticated == true)
            return Array.Empty<object>();

        var roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type.EndsWith("/role"))
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var supplierIdClaim = user.FindFirstValue("SupplierId");
        var employeeIdClaim = user.FindFirstValue("EmployeeId");

        int? supplierId = int.TryParse(supplierIdClaim, out var sId) ? sId : null;
        int? employeeId = int.TryParse(employeeIdClaim, out var eId) ? eId : null;

        var query = Db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
            .ThenInclude(d => d.Product)
            .OrderByDescending(o => o.OrderDate)
            .AsQueryable();

        // 🔹 Employee: endast sina ordrar
        if (roles.Contains("Employee") && employeeId.HasValue)
        {
            query = query.Where(o => o.EmployeeId == employeeId.Value);
        }

        // 🔹 Supplier: endast ordrar som innehåller hans produkter
        else if (roles.Contains("Supplier") && supplierId.HasValue)
        {
            query = query.Where(o => o.OrderDetails.Any(d => d.Product.SupplierId == supplierId.Value));
        }

        // 🔹 Manager/Admin → inga filter

        var data = await query
            .Take(top)
            .Select(o => new
            {
                orderId = o.OrderId,
                customer = o.Customer!.CompanyName,
                employee = o.Employee != null ? o.Employee.FirstName + " " + o.Employee.LastName : "(Ingen)",
                date = o.OrderDate,
                total = Math.Round(
                    o.OrderDetails
                        .Where(d => !supplierId.HasValue || d.Product.SupplierId == supplierId.Value)
                        .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount)),
                    1)
            })
            .AsNoTracking()
            .ToListAsync(ct);

        return data;
    }
}
