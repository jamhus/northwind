using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Orders;

[Authorize]
public class GetOrderById : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult<OrderDetailsDto>
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _contextAccessor;

    public GetOrderById(NorthwindContext db, IHttpContextAccessor contextAccessor)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }

    [HttpGet("api/orders/{id}")]
    public override async Task<ActionResult<OrderDetailsDto>> HandleAsync(int id, CancellationToken ct = default)
    {
        var user = _contextAccessor.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role);
        var supplierIdClaim = user?.FindFirstValue("SupplierId");

        var order = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                    .ThenInclude(p => p.Supplier)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == id, ct);

        if (order is null) return NotFound();

        if (role == "Supplier" && int.TryParse(supplierIdClaim, out var supplierId))
        {
            if (!order.OrderDetails.Any(d => d.Product!.SupplierId == supplierId))
                return Forbid();
        }

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
