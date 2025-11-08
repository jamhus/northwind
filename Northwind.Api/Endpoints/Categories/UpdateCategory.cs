using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Auth;
using Northwind.Helpers;
using Northwind.Models.Data;

namespace Northwind.Endpoints.Categories;

[Authorize(Roles = Roles.Admin)]
public class UpdateCategory : EndpointBaseAsync
    .WithRequest<UpdateCategoryRequest>
    .WithActionResult<CategoryDto>
{
    private readonly NorthwindContext _db;
    public UpdateCategory(NorthwindContext db) => _db = db;

    [HttpPut("api/categories/{id}")]
    public override async Task<ActionResult<CategoryDto>> HandleAsync(
        [FromMultiSource] UpdateCategoryRequest request,
        CancellationToken ct = default)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == request.Id, ct);
        if (category is null)
            return NotFound();

        category.CategoryName = request.Model.CategoryName;
        category.Description = request.Model.Description;

        await _db.SaveChangesAsync(ct);

        var dto = new CategoryDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        };

        return Ok(dto);
    }
}

public class UpdateCategoryRequest
{
    [FromRoute]
    public int Id { get; set; }
    [FromBody]
    public CreateOrUpdateCategoryDto Model { get; set; }
}
