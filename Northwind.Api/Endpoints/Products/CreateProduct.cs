using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Northwind.Endpoints.Products;

public class CreateProduct : EndpointBaseAsync
    .WithRequest<CreateOrUpdateProductDto>
    .WithActionResult<ProductDto>
{
    private readonly NorthwindContext _db;
    public CreateProduct(NorthwindContext db) => _db = db;

    [HttpPost("api/products", Name = nameof(CreateProduct))]
    public override async Task<ActionResult<ProductDto>> HandleAsync(CreateOrUpdateProductDto request, CancellationToken ct = default)
    {
        var product = new Product
        {
            ProductName = request.ProductName,
            UnitPrice = request.UnitPrice,
            UnitsInStock = request.UnitsInStock,
            CategoryId = request.CategoryId,
            SupplierId = request.SupplierId,
            QuantityPerUnit = request.QuantityPerUnit
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);

        return Ok(product);
    }
}
