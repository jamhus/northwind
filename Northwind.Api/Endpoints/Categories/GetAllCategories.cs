using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Endpoints.Categories;

public class GetAllCategories : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<CategoryDto>>
{
    private readonly NorthwindContext _db;
    public GetAllCategories(NorthwindContext db) => _db = db;

    [HttpGet("api/categories", Name = nameof(GetAllCategories))]
    public override async Task<ActionResult<List<CategoryDto>>> HandleAsync(CancellationToken ct = default)
    {
        var categories = await _db.Categories
            .OrderBy(c => c.CategoryName)
            .Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            })
            .ToListAsync(ct);

        return Ok(categories);
    }
}

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
}
