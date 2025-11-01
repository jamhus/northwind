using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Northwind.Endpoints.Products;

public class Create : EndpointBaseAsync
    .WithRequest<Product>
    .WithActionResult<Product>
{
    private readonly NorthwindContext _db;
    public Create(NorthwindContext db) => _db = db;

    [HttpPost("api/products", Name = nameof(Create))]
    public override async Task<ActionResult<Product>> HandleAsync(Product request, CancellationToken ct = default)
    {
        _db.Products.Add(request);
        await _db.SaveChangesAsync(ct);

        return CreatedAtRoute(nameof(GetById), new { id = request.ProductId }, request);
    }
}
