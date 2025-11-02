using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Helpers;
using Northwind.Models;

namespace Northwind.Endpoints.Products;

public class UpdateProduct : EndpointBaseAsync
    .WithRequest<UpdateProductRequest>
    .WithActionResult<ProductDto>
{
    private readonly NorthwindContext _db;
    public UpdateProduct(NorthwindContext db) => _db = db;

    [HttpPut("api/products/{id}", Name = nameof(UpdateProduct))]
    public override async Task<ActionResult<ProductDto>> HandleAsync(
        [FromMultiSource] UpdateProductRequest request,
        CancellationToken ct = default)
    {
        var product = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.ProductId == request.Id, ct);

        if (product is null)
            return NotFound($"Produkt med ID {request.Id} hittades inte.");

        // Uppdatera fält
        product.ProductName = request.Model.ProductName;
        product.UnitPrice = request.Model.UnitPrice;
        product.UnitsInStock = request.Model.UnitsInStock;
        product.CategoryId = request.Model.CategoryId;
        product.SupplierId = request.Model.SupplierId;

        await _db.SaveChangesAsync(ct);

        var dto = new ProductDto
        {
            Id = product.ProductId,
            ProductName = product.ProductName!,
            UnitPrice = product.UnitPrice,
            UnitsInStock = product.UnitsInStock,
            CategoryName = product.Category?.CategoryName ?? "",
            SupplierName = product.Supplier?.CompanyName ?? ""
        };

        return Ok(dto);
    }
}

public class UpdateProductRequest
{
    [FromRoute]
    public int Id { get; set; }
    [FromBody]
    public CreateOrUpdateProductDto Model { get; set; }

}

public class CreateOrUpdateProductDto
{
    [FromBody]
    public string ProductName { get; set; } = "";

    [FromBody]
    public decimal? UnitPrice { get; set; }

    [FromBody]
    public short? UnitsInStock { get; set; }

    [FromBody]
    public int? CategoryId { get; set; }
    [FromBody]
    public int? SupplierId { get; set; }
}
