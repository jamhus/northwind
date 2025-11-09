using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Dashboard;
[Authorize]
public class OrdersDashboard : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<OrdersDashboardResponse>
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrdersDashboard(NorthwindContext db, IHttpContextAccessor accessor)
    {
        _db = db;
        _httpContextAccessor = accessor;
    }

    [HttpGet("api/dashboard/orders")]
    public override async Task<ActionResult<OrdersDashboardResponse>> HandleAsync(CancellationToken ct = default)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role);
        var supplierIdClaim = user?.FindFirstValue("SupplierId");
        var employeeIdClaim = user?.FindFirstValue("EmployeeId");

        var orders = _db.Orders
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)!.ThenInclude(p => p.Supplier)
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .AsNoTracking()
            .AsQueryable();


        if (role == "Supplier" && int.TryParse(supplierIdClaim, out var supplierId))
        {
            orders = orders.Where(o => o.OrderDetails.Any(d => d.Product!.SupplierId == supplierId));
        }
        else if (role == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
        {
            orders = orders.Where(o => o.EmployeeId == employeeId);
        }


        var orderList = await orders.ToListAsync(ct);
        var details = orderList.SelectMany(o => o.OrderDetails);

        decimal totalSales = details.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount));
        int orderCount = orderList.Count;
        int customerCount = orderList.Select(o => o.CustomerId).Distinct().Count();

        // Försäljning per månad
        var salesByMonth = orderList
            .Where(o => o.OrderDate.HasValue)
            .GroupBy(o => new { o.OrderDate!.Value.Year, o.OrderDate.Value.Month })
            .Select(g => new SalesByMonthDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:00}",
                TotalSales = g.SelectMany(o => o.OrderDetails)
                    .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .OrderBy(x => x.Month)
            .ToList();

        // Top Products
        var topProducts = details
            .Where(d => d.Product != null)
            .GroupBy(d => d.Product!.ProductName)
            .Select(g => new TopItemDto
            {
                Name = g.Key,
                TotalSales = g.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(5)
            .ToList();

        // Top Customers
        var topCustomers = orderList
            .Where(o => o.Customer != null)
            .GroupBy(o => o.Customer!.CompanyName)
            .Select(g => new TopItemDto
            {
                Name = g.Key,
                TotalSales = g.SelectMany(o => o.OrderDetails)
                    .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(5)
            .ToList();

        // Sales by Region
        var salesByRegion = orderList
            .Where(o => o.ShipCountry != null)
            .GroupBy(o => o.ShipCountry!)
            .Select(g => new SalesByRegionDto
            {
                Region = g.Key,
                TotalSales = g.SelectMany(o => o.OrderDetails)
                    .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
            })
            .OrderByDescending(x => x.TotalSales)
            .ToList();

        // Performance per Employee
        List<EmployeePerformanceDto> performance = new();

        if (role == "Admin")
        {
            performance = orderList
                .Where(o => o.Employee != null)
                .GroupBy(o => o.Employee!.FirstName + " " + o.Employee!.LastName)
                .Select(g => new EmployeePerformanceDto
                {
                    EmployeeName = g.Key,
                    TotalSales = g.SelectMany(o => o.OrderDetails)
                        .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount)),
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();
        }

        else if (role == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
        {
            var ownOrders = orderList.Where(o => o.EmployeeId == employeeId).ToList();
            var totalSalesEmp = ownOrders.SelectMany(o => o.OrderDetails)
                .Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount));

            performance = new List<EmployeePerformanceDto>
            {
                new()
                {
                    EmployeeName = ownOrders.FirstOrDefault()?.Employee?.FirstName + " " +
                                   ownOrders.FirstOrDefault()?.Employee?.LastName,
                    TotalSales = totalSalesEmp,
                    OrderCount = ownOrders.Count
                }
            };
        }




        var response = new OrdersDashboardResponse
        {
            TotalSales = totalSales,
            OrderCount = orderCount,
            CustomerCount = customerCount,
            SalesByMonth = salesByMonth,
            TopProducts = topProducts,
            TopCustomers = topCustomers,
            SalesByRegion = salesByRegion,
            Performance = performance
        };

        // Senaste ordrarna (endast Employee)
        List<RecentOrderDto> recentOrders = new();
        if (role == "Employee" && int.TryParse(employeeIdClaim, out var empId))
        {
            recentOrders = orderList
                .Where(o => o.EmployeeId == empId)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .Select(o => new RecentOrderDto
                {
                    OrderId = o.OrderId,
                    Customer = o.Customer!.CompanyName,
                    Date = o.OrderDate ?? DateTime.MinValue,
                    Total = o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount))
                })
                .ToList();

            response.RecentOrders = recentOrders;
        }


        return Ok(response);
    }
}



public class OrdersDashboardResponse
{
    public decimal TotalSales { get; set; }
    public int OrderCount { get; set; }
    public int CustomerCount { get; set; }
    public List<SalesByMonthDto> SalesByMonth { get; set; } = [];
    public List<TopItemDto> TopProducts { get; set; } = [];
    public List<TopItemDto> TopCustomers { get; set; } = [];
    public List<SalesByRegionDto> SalesByRegion { get; set; } = [];
    public List<EmployeePerformanceDto> Performance { get; set; } = [];
    public List<RecentOrderDto> RecentOrders { get; set; } = [];
}

public class RecentOrderDto
{
    public int OrderId { get; set; }
    public string Customer { get; set; } = "";
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
}


public class SalesByMonthDto
{
    public string Month { get; set; } = "";
    public decimal TotalSales { get; set; }
}

public class TopItemDto
{
    public string Name { get; set; } = "";
    public decimal TotalSales { get; set; }
}

public class SalesByRegionDto
{
    public string Region { get; set; } = "";
    public decimal TotalSales { get; set; }
}

public class EmployeePerformanceDto
{
    public string EmployeeName { get; set; } = "";
    public decimal TotalSales { get; set; }
    public int OrderCount { get; set; }
}
