using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Auth;
using Northwind.Models.Data;

namespace Northwind.Endpoints.Products;
[Authorize(Roles = Roles.Admin)]

public class DeleteProduct : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult
{
    private readonly NorthwindContext _db;
    public DeleteProduct(NorthwindContext db) => _db = db;

    [HttpDelete("api/products/{id}", Name = nameof(DeleteProduct))]
    public override async Task<ActionResult> HandleAsync(int id, CancellationToken ct = default)
    {
        var product = await _db.Products.FindAsync(new object[] { id }, ct);
        if (product is null)
            return NotFound();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }
}
