using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Endpoints.Categories;

public class GetAllSuppliersForSelect : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<SupplierListItemDto>>
{
    private readonly NorthwindContext _db;
    public GetAllSuppliersForSelect(NorthwindContext db) => _db = db;

    [HttpGet("api/suppliers/for-select", Name = nameof(GetAllSuppliersForSelect))]
    public override async Task<ActionResult<List<SupplierListItemDto>>> HandleAsync(CancellationToken ct = default)
    {
        var suppliers = await _db.Suppliers
            .OrderBy(s => s.CompanyName)
            .Select(s => new SupplierListItemDto
            {
                SupplierId = s.SupplierId,
                CompanyName = s.CompanyName,
            })
            .OrderByDescending(s => s.CompanyName)
            .ToListAsync(ct);

        return Ok(suppliers);
    }
}

public class SupplierListItemDto
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}
