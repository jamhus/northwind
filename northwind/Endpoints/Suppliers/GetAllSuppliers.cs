using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Endpoints.Categories;

public class GetAllSuppliers : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<SupplierDto>>
{
    private readonly NorthwindContext _db;
    public GetAllSuppliers(NorthwindContext db) => _db = db;

    [HttpGet("api/suppliers", Name = nameof(GetAllSuppliers))]
    public override async Task<ActionResult<List<SupplierDto>>> HandleAsync(CancellationToken ct = default)
    {
        var suppliers = await _db.Suppliers
            .OrderBy(s => s.CompanyName)
            .Select(s => new SupplierDto
            {
                SupplierId = s.SupplierId,
                SupplierName = s.CompanyName
            })
            .ToListAsync(ct);

        return Ok(suppliers);
    }
}

public class SupplierDto
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = "";
}
