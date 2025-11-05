using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Helpers;
using Northwind.Models;

namespace Northwind.Endpoints.Suppliers;

public class GetAllSuppliers : EndpointBaseAsync
    .WithRequest<ListRequest>
    .WithActionResult<PagedResult<SupplierDto>>
{
    private readonly NorthwindContext _db;
    public GetAllSuppliers(NorthwindContext db) => _db = db;

    [HttpGet("api/suppliers", Name = nameof(GetAllSuppliers))]
    public override async Task<ActionResult<PagedResult<SupplierDto>>> HandleAsync(
        [FromQuery] ListRequest request,
        CancellationToken ct = default)
    {
        var result = await _db.Suppliers
            .OrderBy(s => s.SupplierId)
            .Select(s => new SupplierDto
            {
                SupplierId = s.SupplierId,
                CompanyName = s.CompanyName,
                ContactName = s.ContactName,
                ContactTitle = s.ContactTitle,
            })
            .OrderByDescending(s => s.SupplierId)
            .ToPagedResultAsync(request.Page, request.PageSize, ct);

        return Ok(result);
    }
}
public class SupplierDto
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
}