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

        var query = Db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
            .ThenInclude(d => d.Product)
            .OrderByDescending(o => o.OrderDate)
            .AsQueryable();

        // 🔹 Employee: se sina egna orders
        if (roles.Contains("Employee"))
        {
            var employeeId = user.FindFirstValue("EmployeeId");
            if (int.TryParse(employeeId, out var id))
                query = query.Where(o => o.EmployeeId == id);
            else
                return Array.Empty<object>();
        }

        // 🔹 Supplier: se orders med deras produkter
        else if (roles.Contains("Supplier"))
        {
            var supplierId = user.FindFirstValue("SupplierId");
            if (int.TryParse(supplierId, out var sid))
                query = query.Where(o => o.OrderDetails.Any(d => d.Product.SupplierId == sid));
        }

        // 🔹 Manager: ser alla orders (Northwinds egna)
        else if (roles.Contains("Manager"))
        {
            // Managers är Northwind-personal → ser allt
        }

        // 🔹 Admin: ser allt

        var data = await query
            .Take(top)
            .Select(o => new
            {
                orderId = o.OrderId,
                customer = o.Customer!.CompanyName,
                employee = o.Employee != null ? o.Employee.FirstName + " " + o.Employee.LastName : "(Ingen)",
                date = o.OrderDate,
                total = Math.Round(
                    o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount)),
                    1)
            })
            .AsNoTracking()
            .ToListAsync(ct);

        return data;
    }
}
