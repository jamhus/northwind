using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Helpers;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Orders;

[Authorize]
public class GetAllOrders : EndpointBaseAsync
    .WithRequest<ListRequest>
    .WithActionResult<PagedResult<OrderDto>>
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _contextAccessor;

    public GetAllOrders(NorthwindContext db, IHttpContextAccessor contextAccessor)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }

    [HttpGet("api/orders")]
    public override async Task<ActionResult<PagedResult<OrderDto>>> HandleAsync(
        [FromQuery] ListRequest request,
        CancellationToken ct = default)
    {
        var user = _contextAccessor.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role) ?? "";
        var supplierIdClaim = user?.FindFirstValue("SupplierId");

        var query = _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .AsNoTracking();

        if (role == "Supplier" && int.TryParse(supplierIdClaim, out var supplierId))
        {
            query = query.Where(o => o.OrderDetails.Any(od => od.Product!.SupplierId == supplierId));
        }

        var result = await query
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                CustomerName = o.Customer!.CompanyName,
                EmployeeName = o.Employee!.FirstName + " " + o.Employee.LastName,
                ShipCountry = o.ShipCountry,
                Total = o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity * (1 - (decimal)d.Discount)),
            })
            .ToPagedResultAsync(request.Page, request.PageSize, ct);

        return Ok(result);
    }
}

public class OrderDto
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public string CustomerName { get; set; } = "";
    public string EmployeeName { get; set; } = "";
    public string? ShipCountry { get; set; }
    public decimal Total { get; set; }
}
