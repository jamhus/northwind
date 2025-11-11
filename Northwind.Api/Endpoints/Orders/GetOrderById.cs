using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Orders;

public class GetOrderById : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult<object>
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _http;

    public GetOrderById(NorthwindContext db, IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    [HttpGet("api/orders/{id:int}")]
    public override async Task<ActionResult<object>> HandleAsync(int id, CancellationToken ct = default)
    {
        var user = _http.HttpContext!.User;
        var roles = user.Claims
          .Where(c => c.Type == ClaimTypes.Role || c.Type.EndsWith("/role"))
          .Select(c => c.Value)
          .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var employeeIdClaim = user.FindFirstValue("EmployeeId");
        var supplierIdClaim = user.FindFirstValue("SupplierId");
        int? supplierId = int.TryParse(supplierIdClaim, out var sId) ? sId : null;
        int? employeeId = int.TryParse(employeeIdClaim, out var eId) ? eId : null;

        var query = _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Supplier)
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

        var order = await query.FirstOrDefaultAsync(o => o.OrderId == id, ct);

        if (order == null) return NotFound();

        if (roles.Contains("Supplier") && supplierId.HasValue)
        {
            if (!order.OrderDetails.Any(d => d.Product.SupplierId == supplierId.Value))
                return Forbid();

            order.OrderDetails = order.OrderDetails
                .Where(d => d.Product.SupplierId == supplierId.Value)
                .ToList();
        }

        if (order == null) return NotFound();

        var dto = new OrderDetailsDto
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            CustomerName = order.Customer!.CompanyName,
            EmployeeName = $"{order.Employee!.FirstName} {order.Employee.LastName}",
            ShipCountry = order.ShipCountry,
            Details = order.OrderDetails.Select(d => new OrderItemDto
            {
                ProductName = d.Product!.ProductName,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Discount = d.Discount,
                SupplierName = d.Product.Supplier!.CompanyName
            }).ToList()
        };


        return Ok(dto);
    }
}


public class OrderDetailsDto
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public string CustomerName { get; set; } = "";
    public string EmployeeName { get; set; } = "";
    public string? ShipCountry { get; set; }
    public List<OrderItemDto> Details { get; set; } = new();
}

public class OrderItemDto
{
    public string ProductName { get; set; } = "";
    public short Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public float Discount { get; set; }
    public string SupplierName { get; set; } = "";
}
