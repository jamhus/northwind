using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Endpoints.Categories;

public class GetAllCategoriesForSelect : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<CategoryListItemDto>>
{
    private readonly NorthwindContext _db;
    public GetAllCategoriesForSelect(NorthwindContext db) => _db = db;

    [HttpGet("api/categories/for-select", Name = nameof(GetAllCategoriesForSelect))]
    public override async Task<ActionResult<List<CategoryListItemDto>>> HandleAsync(CancellationToken ct = default)
    {
        var suppliers = await _db.Categories
            .OrderBy(c => c.CategoryName)
            .Select(c => new CategoryListItemDto
            {
                CategoryId  = c.CategoryId,
                CategoryName = c.CategoryName,
            })
            .ToListAsync(ct);

        return Ok(suppliers);
    }
}

public class CategoryListItemDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
}
