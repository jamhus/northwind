using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Helpers;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Products;

[Authorize(Roles = "Admin,Supplier")]
public class UpdateProduct : EndpointBaseAsync
    .WithRequest<UpdateProductRequest>
    .WithActionResult<ProductDto>
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _contextAccessor;

    public UpdateProduct(NorthwindContext db, IHttpContextAccessor contextAccessor)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }

    [HttpPut("api/products/{id}")]
    public override async Task<ActionResult<ProductDto>> HandleAsync(
        [FromMultiSource] UpdateProductRequest request,
        CancellationToken ct = default)
    {
        var user = _contextAccessor.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role) ?? "";
        var supplierIdClaim = user?.FindFirstValue("SupplierId");

        var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == request.Id, ct);
        if (product is null) return NotFound();

        if (role.Contains("Supplier") && supplierIdClaim != null)
        {
            if (product.SupplierId.ToString() != supplierIdClaim)
                return Forbid();
        }

        product.ProductName = request.Model.ProductName;
        product.UnitPrice = request.Model.UnitPrice;
        product.UnitsInStock = request.Model.UnitsInStock;
        product.CategoryId = request.Model.CategoryId;
        product.QuantityPerUnit = request.Model.QuantityPerUnit;

        await _db.SaveChangesAsync(ct);

        return Ok(new ProductDto
        {
            Id = product.ProductId,
            ProductName = product.ProductName,
            UnitPrice = product.UnitPrice,
            UnitsInStock = product.UnitsInStock
        });
    }
}

public class UpdateProductRequest
{
    [FromRoute] public int Id { get; set; }
    [FromBody] public CreateOrUpdateProductDto Model { get; set; } = default!;
}
public class CreateOrUpdateProductDto
{
    public string ProductName { get; set; } = "";

    public decimal? UnitPrice { get; set; }

    public short? UnitsInStock { get; set; }

    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public string? QuantityPerUnit { get; set; }

}
