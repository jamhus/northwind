using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Dashboard.Handlers;

public class TopCustomersHandler : BaseHandler
{
    private readonly IHttpContextAccessor _http;

    public TopCustomersHandler(NorthwindContext db, IHttpContextAccessor http) : base(db)
    {
        _http = http;
    }

    public override string Type => "TopCustomers";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var user = _http.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role);
        var supplierId = user?.FindFirstValue("SupplierId");
        var employeeId = user?.FindFirstValue("EmployeeId");

        var top = GetTopArg(settings, 5);

        var query = Db.OrderDetails
            .Include(d => d.Order)
            .Include(d => d.Product)
            .Where(d => d.Order != null && d.Order.Customer != null)
            .AsQueryable();

        // 🔹 Supplier: endast deras produkter
        if (role == "Supplier" && int.TryParse(supplierId, out var sid))
        {
            query = query.Where(d => d.Product.SupplierId == sid);
        }
        // 🔹 Employee: endast ordrar som tillhör honom
        else if (role == "Employee" && int.TryParse(employeeId, out var eid))
        {
            query = query.Where(d => d.Order.EmployeeId == eid);
        }

        var data = await query
            .GroupBy(d => new { d.Order!.CustomerId, d.Order!.Customer!.CompanyName })
            .Select(g => new
            {
                Customer = g.Key.CompanyName,
                TotalSales = g.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(top)
            .AsNoTracking()
            .ToListAsync(ct);

        return data.Select(x => new
        {
            customer = x.Customer,
            totalSales = Math.Round(x.TotalSales, 1)
        }).ToList();
    }
}
