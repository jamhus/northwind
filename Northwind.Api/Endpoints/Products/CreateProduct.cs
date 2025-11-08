using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Products;

[Authorize(Roles = "Admin,Supplier")]
public class CreateProduct : EndpointBaseAsync
    .WithRequest<CreateOrUpdateProductDto>
    .WithActionResult<ProductDto>
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreateProduct(NorthwindContext db, IHttpContextAccessor contextAccessor)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }

    [HttpPost("api/products")]
    public override async Task<ActionResult<ProductDto>> HandleAsync(
        [FromBody] CreateOrUpdateProductDto request,
        CancellationToken ct = default)
    {
        var user = _contextAccessor.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role) ?? "";
        var supplierIdClaim = user?.FindFirstValue("SupplierId");

        int? supplierId = null;
        if (role.Contains("Supplier") && int.TryParse(supplierIdClaim, out var id))
            supplierId = id;
        else if (request.SupplierId.HasValue)
            supplierId = request.SupplierId;

        var product = new Product
        {
            ProductName = request.ProductName,
            UnitPrice = request.UnitPrice,
            UnitsInStock = request.UnitsInStock,
            CategoryId = request.CategoryId,
            SupplierId = supplierId,
            QuantityPerUnit = request.QuantityPerUnit
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);

        var dto = new ProductDto
        {
            Id = product.ProductId,
            ProductName = product.ProductName,
            UnitPrice = product.UnitPrice,
            UnitsInStock = product.UnitsInStock,
            CategoryName = (await _db.Categories.FindAsync(product.CategoryId))?.CategoryName ?? "",
        };

        return Ok(dto);
    }
}
