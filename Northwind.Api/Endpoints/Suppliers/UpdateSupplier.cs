using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Endpoints.Categories;
using Northwind.Helpers;
using Northwind.Models;

namespace Northwind.Endpoints.Suppliers;

public class UpdateSupplier : EndpointBaseAsync
    .WithRequest<UpdateSupplierRequest>
    .WithActionResult<SupplierDto>
{
    private readonly NorthwindContext _db;
    public UpdateSupplier(NorthwindContext db) => _db = db;

    [HttpPut("api/suppliers/{id}")]
    public override async Task<ActionResult<SupplierDto>> HandleAsync(
        [FromMultiSource] UpdateSupplierRequest request,

        CancellationToken ct = default)
    {
        var supplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == request.Id, ct);
        if (supplier is null)
            return NotFound();

        supplier.CompanyName = request.Model.CompanyName;
        supplier.ContactName = request.Model.ContactName;
        supplier.ContactTitle = request.Model.ContactTitle;

        await _db.SaveChangesAsync(ct);

        return Ok(request);
    }
}
public class UpdateSupplierRequest
{
    [FromRoute]
    public int Id { get; set; }
    [FromBody]
    public CreateOrUpdateSupplierDto Model { get; set; }
}
