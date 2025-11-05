using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Northwind.Endpoints.Categories;

public class CreateCategory : EndpointBaseAsync
    .WithRequest<CreateOrUpdateCategoryDto>
    .WithActionResult<CategoryDto>
{
    private readonly NorthwindContext _db;
    public CreateCategory(NorthwindContext db) => _db = db;

    [HttpPost("api/categories")]
    public override async Task<ActionResult<CategoryDto>> HandleAsync(
        [FromBody] CreateOrUpdateCategoryDto request,
        CancellationToken ct = default)
    {
        var entity = new Category
        {
            CategoryName = request.CategoryName,
            Description = request.Description
        };

        _db.Categories.Add(entity);
        await _db.SaveChangesAsync(ct);

        var dto = new CategoryDto
        {
            CategoryId = entity.CategoryId,
            CategoryName = entity.CategoryName,
            Description = entity.Description
        };

        return Ok(dto);
    }
}

public class CreateOrUpdateCategoryDto : CategoryDto
{
}
