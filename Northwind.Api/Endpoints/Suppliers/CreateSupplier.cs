using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Endpoints.Suppliers;

public class CreateSupplier : EndpointBaseAsync
    .WithRequest<CreateOrUpdateSupplierDto>
    .WithActionResult<SupplierDto>
{
    private readonly NorthwindContext _db;
    public CreateSupplier(NorthwindContext db) => _db = db;

    [HttpPost("api/suppliers")]
    public override async Task<ActionResult<SupplierDto>> HandleAsync(
        [FromBody] CreateOrUpdateSupplierDto request,
        CancellationToken ct = default)
    {
        var entity = new Supplier
        {
            CompanyName = request.CompanyName,
            ContactName = request.ContactName,
            ContactTitle = request.ContactTitle,
        };

        _db.Suppliers.Add(entity);
        await _db.SaveChangesAsync(ct);

        var dto = new SupplierDto
        {
            SupplierId = entity.SupplierId,
            CompanyName = entity.CompanyName,
            ContactName = entity.ContactName,
            ContactTitle = entity.ContactTitle,
        };

        return Ok(dto);
    }
}

public class CreateOrUpdateSupplierDto : SupplierDto
{
}
