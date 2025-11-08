using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Helpers;
using Northwind.Models;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Products;

[Authorize]
public class GetAll : EndpointBaseAsync
    .WithRequest<ListRequest>
    .WithActionResult<PagedResult<ProductDto>>
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAll(NorthwindContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("api/products")]
    public override async Task<ActionResult<PagedResult<ProductDto>>> HandleAsync(
        [FromQuery] ListRequest request,
        CancellationToken ct = default)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var role = user?.FindFirstValue("Role") ?? "";
        var supplierIdClaim = user?.FindFirstValue("SupplierId");

        IQueryable<Product> query = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsNoTracking();

        if (role.Contains("Supplier") && int.TryParse(supplierIdClaim, out var supplierId))
        {
            query = query.Where(p => p.SupplierId == supplierId);
        }

        var result = await query
            .OrderByDescending(p => p.ProductId)
            .Select(p => new ProductDto
            {
                Id = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                CategoryName = p.Category.CategoryName,
                CompanyName = p.Supplier.CompanyName,
                QuantityPerUnit = p.QuantityPerUnit,
                SupplierId = p.Supplier.SupplierId,
                CategoryId = p.Category.CategoryId
            })
            .ToPagedResultAsync(request.Page, request.PageSize, ct);

        return Ok(result);
    }
}


public class ProductDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
    public string CompanyName { get; set; }
    public int SupplierId { get; set; }
    public string? QuantityPerUnit { get; set; }

}
