using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Dashboard.Handlers;

public class SalesPerMonthHandler : BaseHandler
{
    private readonly IHttpContextAccessor _http;

    public SalesPerMonthHandler(NorthwindContext db, IHttpContextAccessor http) : base(db)
    {
        _http = http;
    }

    public override string Type => "SalesPerMonth";

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
            .Where(d => d.Order!.OrderDate != null)
            .AsQueryable();

        if (role == "Supplier" && int.TryParse(supplierId, out var sid))
        {
            query = query.Where(d => d.Product.SupplierId == sid);
        }
        else if (role == "Employee" && int.TryParse(employeeId, out var eid))
        {
            query = query.Where(d => d.Order.EmployeeId == eid);
        }

        var result = await query
            .GroupBy(d => new
            {
                d.Order!.OrderDate!.Value.Year,
                d.Order!.OrderDate!.Value.Month
            })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalSales = g.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .OrderBy(g => g.Year).ThenBy(g => g.Month)
            .AsNoTracking()
            .ToListAsync(ct);

        // avrunda till 1 decimal
        return result.Select(x => new
        {
            year = x.Year,
            month = x.Month,
            totalSales = Math.Round(x.TotalSales, 1)
        }).ToList();
    }
}
