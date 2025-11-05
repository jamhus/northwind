using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Helpers;
using Northwind.Models;

namespace Northwind.Endpoints.Categories;

public class GetAllCategories : EndpointBaseAsync
     .WithRequest<ListRequest>
    .WithActionResult<List<CategoryDto>>
{
    private readonly NorthwindContext _db;
    public GetAllCategories(NorthwindContext db) => _db = db;

    [HttpGet("api/categories", Name = nameof(GetAllCategories))]
    public override async Task<ActionResult<List<CategoryDto>>> HandleAsync([FromQuery] ListRequest request, CancellationToken ct = default)
    {
        var categories = await _db.Categories
            .OrderBy(c => c.CategoryName)
            .Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            })
            .ToPagedResultAsync(request.Page, request.PageSize);

        return Ok(categories);
    }
}

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string Description { get; set; }
}
