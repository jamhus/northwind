using System.Security.Claims;

namespace Northwind.Dashboard.Engine;

public class DynamicDataService
{
    private readonly IHttpContextAccessor _http;

    public DynamicDataService(IHttpContextAccessor http)
    {
        _http = http;
    }

    private ClaimsPrincipal? User => _http.HttpContext?.User;

    private bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    // 🔹 Hämtar aktuella roller som en kommaseparerad sträng
    public string GetCurrentRoles()
    {
        if (User == null || !User.Identity?.IsAuthenticated == true)
            return "Guest";

        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type.EndsWith("/role"))
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase);

        return string.Join(",", roles);
    }


    // 🔹 Kontroll om användaren är admin
    public bool GetIsAdmin()
    {
        return GetCurrentRoles()
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Any(r => r.Equals("Admin", StringComparison.OrdinalIgnoreCase));
    }

    // 🔹 Employee ID (om användaren är en anställd)
    public int? GetCurrentEmployeeId()
    {
        if (!IsAuthenticated)
            return null;

        var claim = User?.Claims.FirstOrDefault(c => c.Type == "EmployeeId");
        if (int.TryParse(claim?.Value, out var id))
            return id;

        return null;
    }

    // 🔹 Supplier ID (om användaren är kopplad till ett företag)
    public int? GetSupplierId()
    {
        if (!IsAuthenticated)
            return null;

        var claim = User?.Claims.FirstOrDefault(c => c.Type == "SupplierId");
        if (int.TryParse(claim?.Value, out var id))
            return id;

        return null;
    }

    // 🔹 Company ID (om du har separata companies)
    public int? GetCurrentCompanyId()
    {
        if (!IsAuthenticated)
            return 1; // Northwind default

        var claim = User?.Claims.FirstOrDefault(c => c.Type == "CompanyId");
        if (int.TryParse(claim?.Value, out var id))
            return id;

        return 1;
    }

    // 🔹 Returnerar userId (för loggning eller filtrering)
    public string? GetCurrentUserId()
    {
        return User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
