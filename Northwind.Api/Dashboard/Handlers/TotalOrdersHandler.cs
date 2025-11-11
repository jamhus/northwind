using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Dashboard.Handlers;

public class TotalOrdersHandler : BaseHandler
{
    private readonly IHttpContextAccessor _http;

    public TotalOrdersHandler(NorthwindContext db, IHttpContextAccessor http) : base(db)
    {
        _http = http;
    }

    public override string Type => "TotalOrders";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var user = _http.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role);
        var supplierId = user?.FindFirstValue("SupplierId");
        var employeeId = user?.FindFirstValue("EmployeeId");

        var query = Db.OrderDetails
            .Include(d => d.Order)
            .Include(d => d.Product)
            .AsQueryable();

        if (role == "Supplier" && int.TryParse(supplierId, out var sid))
        {
            // 🔹 Endast orderrader som tillhör supplierns produkter
            query = query.Where(d => d.Product.SupplierId == sid);
        }
        else if (role == "Employee" && int.TryParse(employeeId, out var eid))
        {
            // 🔹 Endast ordrar som skapats av denna anställd
            query = query.Where(d => d.Order.EmployeeId == eid);
        }

        var totalOrders = await query
            .Select(d => d.OrderId)
            .Distinct()
            .CountAsync(ct);

        return totalOrders;
    }
}
