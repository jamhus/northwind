using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Endpoints.Categories;

public class DeleteCategory : EndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
{
    private readonly NorthwindContext _db;
    public DeleteCategory(NorthwindContext db) => _db = db;

    [HttpDelete("api/categories/{id}")]
    public override async Task HandleAsync(int id, CancellationToken ct = default)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id, ct);
        if (category is null) return;

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync(ct);
    }
}
