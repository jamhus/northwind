using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Northwind.Endpoints.Products;

public class CreateProduct : EndpointBaseAsync
    .WithRequest<Product>
    .WithActionResult<Product>
{
    private readonly NorthwindContext _db;
    public CreateProduct(NorthwindContext db) => _db = db;

    [HttpPost("api/products", Name = nameof(CreateProduct))]
    public override async Task<ActionResult<Product>> HandleAsync(Product request, CancellationToken ct = default)
    {
        _db.Products.Add(request);
        await _db.SaveChangesAsync(ct);

        return CreatedAtRoute(nameof(GetProductById), new { id = request.ProductId }, request);
    }
}
