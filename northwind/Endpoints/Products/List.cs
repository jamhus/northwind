using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
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
                .OrderBy(p => p.ProductId)
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