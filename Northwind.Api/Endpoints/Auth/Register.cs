using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Endpoints.Auth;

public class Register : EndpointBaseAsync
    .WithRequest<RegisterRequest>
    .WithActionResult
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly NorthwindContext _db;

    public Register(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, NorthwindContext db)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
    }

    [HttpPost("api/auth/register")]
    public override async Task<ActionResult> HandleAsync(RegisterRequest request, CancellationToken ct = default)
    {
        if (!await _roleManager.RoleExistsAsync(request.Role))
            await _roleManager.CreateAsync(new IdentityRole(request.Role));

        if (request.Role == "Supplier" && request.SupplierId == null)
            return BadRequest("SupplierId krävs för Supplier-användare.");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            SupplierId = request.SupplierId
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, request.Role);
        await _db.SaveChangesAsync(ct);

        return Ok(new { Message = $"User {request.Email} created as {request.Role}" });
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Employee";
    public int? SupplierId { get; set; }
}
