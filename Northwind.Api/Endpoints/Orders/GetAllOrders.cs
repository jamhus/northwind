using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Helpers;
using Northwind.Models.Data;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

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
