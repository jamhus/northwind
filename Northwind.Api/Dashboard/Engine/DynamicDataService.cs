using System.Security.Claims;

namespace Northwind.Dashboard.Engine;

// Service to access dynamic data like user claims.
// This will contain the methods that runs the parameters.
public class DynamicDataService
{
    private readonly IHttpContextAccessor _http;
    public DynamicDataService(IHttpContextAccessor http) => _http = http;

    public bool GetIsAdmin()
    {
        var role = _http.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
    }

    public string? GetClaim(string type) => _http.HttpContext?.User.FindFirstValue(type);
}
