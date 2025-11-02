using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Northwind.Endpoints.Products
{
    public class GetProductById : EndpointBaseAsync
     .WithRequest<int>
     .WithActionResult<Product>
    {
        private readonly NorthwindContext _db;
        public GetProductById(NorthwindContext db) => _db = db;

        [HttpGet("api/products/{id}", Name = nameof(GetProductById))]
        public override async Task<ActionResult<Product>> HandleAsync(int id, CancellationToken ct = default)
        {
            var product = await _db.Products.FindAsync(new object[] { id }, ct);
            return product is not null ? Ok(product) : NotFound();
        }
    }
}
