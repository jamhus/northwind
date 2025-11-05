using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Endpoints.Suppliers;

public class DeleteSupplier : EndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
{
    private readonly NorthwindContext _db;
    public DeleteSupplier(NorthwindContext db) => _db = db;

    [HttpDelete("api/suppliers/{id}")]
    public override async Task HandleAsync(int id, CancellationToken ct = default)
    {
        var supplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id, ct);
        if (supplier is null) return;

        _db.Suppliers.Remove(supplier);
        await _db.SaveChangesAsync(ct);
    }
}
