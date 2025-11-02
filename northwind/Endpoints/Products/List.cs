using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Helpers;
using Northwind.Models;

namespace Northwind.Endpoints.Products
{
    public class GetAll : EndpointBaseAsync
     .WithRequest<ListRequest>
     .WithActionResult<PagedResult<Product>>
    {
        private readonly NorthwindContext _db;
        public GetAll(NorthwindContext db) => _db = db;

        [HttpGet("api/products", Name = nameof(GetAll))]
        public override async Task<ActionResult<PagedResult<Product>>> HandleAsync(
            [FromQuery] ListRequest request,
            CancellationToken ct = default)
        {
            var result = await _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductId)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    CategoryName = p.Category.CategoryName
                })
                .ToPagedResultAsync(request.Page, request.PageSize, ct);

            return Ok(result);
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