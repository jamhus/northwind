using System.Security.Claims;

namespace Northwind.Auth;

public static class UserClaimsExtensions
{
    public static int? GetEmployeeId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue(CustomClaimTypes.EmployeeId), out var v) ? v : (int?)null;

    public static int? GetSupplierId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue(CustomClaimTypes.SupplierId), out var v) ? v : (int?)null;
}
