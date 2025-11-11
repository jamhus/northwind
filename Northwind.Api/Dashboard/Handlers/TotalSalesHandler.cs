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
            query = query.Where(d => d.Product.SupplierId == sid);
        }
        else if (role == "Employee" && int.TryParse(employeeId, out var eid))
        {
            query = query.Where(d => d.Order.EmployeeId == eid);
        }

        var totalSales = await query
            .SumAsync(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount), ct);

        return Math.Round(totalSales, 1);
    }
}
