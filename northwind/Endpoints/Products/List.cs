using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Endpoints.Products
{
    public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<ProductDto>>
    {
        private readonly NorthwindContext _db;
        public List(NorthwindContext db) => _db = db;

        [HttpGet("api/products")]
        public override async Task<ActionResult<List<ProductDto>>> HandleAsync(CancellationToken ct = default)
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null
                })
                .ToListAsync(ct);

            return Ok(products);
        }
    }

    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public string CategoryName { get; set; }
    }
}