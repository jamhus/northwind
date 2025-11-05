using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Helpers;
using Northwind.Models;

namespace Northwind.Endpoints.Products
{
    public class GetAllProducts : EndpointBaseAsync
     .WithRequest<ListRequest>
     .WithActionResult<PagedResult<Product>>
    {
        private readonly NorthwindContext _db;
        public GetAllProducts(NorthwindContext db) => _db = db;

        [HttpGet("api/products", Name = nameof(GetAllProducts))]
        [ProducesResponseType(typeof(PagedResult<ProductDto>), StatusCodes.Status200OK)]

        public override async Task<ActionResult<PagedResult<Product>>> HandleAsync(
            [FromQuery] ListRequest request,
            CancellationToken ct = default)
        {
            var result = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .OrderBy(p => p.ProductId)
                .Select(p => new ProductDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    CategoryName = p.Category.CategoryName,
                    CategoryId = (int)p.CategoryId,
                    SupplierName = p.Supplier.CompanyName,
                    SupplierId = (int)p.SupplierId,
                    QuantityPerUnit = p.QuantityPerUnit
                })
                .OrderByDescending(P => P.Id)
                .ToPagedResultAsync(request.Page, request.PageSize, ct);

            return Ok(result);
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public string? QuantityPerUnit { get; set; }

    }
}