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
    public IEnumerable<string> GetCurrentRoles()
    {
        var user = _http.HttpContext?.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
            return Enumerable.Empty<string>();

        // Hämtar alla roller från claims
        var roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type.EndsWith("/role"))
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Om ingen roll hittas, anta "Guest"
        if (!roles.Any())
            roles.Add("Guest");

        return roles;
    }

    public string? GetClaim(string type) => _http.HttpContext?.User.FindFirstValue(type);
}
