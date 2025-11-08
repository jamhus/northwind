using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models.Data;
using System.Security.Claims;

namespace Northwind.Endpoints.Products;

[Authorize(Roles = "Admin,Supplier")]
public class DeleteProduct : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult
{
    private readonly NorthwindContext _db;
    private readonly IHttpContextAccessor _contextAccessor;

    public DeleteProduct(NorthwindContext db, IHttpContextAccessor contextAccessor)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }

    [HttpDelete("api/products/{id}")]
    public override async Task<ActionResult> HandleAsync(int id, CancellationToken ct = default)
    {
        var user = _contextAccessor.HttpContext?.User;
        var role = user?.FindFirstValue(ClaimTypes.Role) ?? "";
        var supplierIdClaim = user?.FindFirstValue("SupplierId");

        var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id, ct);
        if (product is null) return NotFound();

        if (role.Contains("Supplier") && supplierIdClaim != null)
        {
            if (product.SupplierId.ToString() != supplierIdClaim)
                return Forbid();
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }
}
