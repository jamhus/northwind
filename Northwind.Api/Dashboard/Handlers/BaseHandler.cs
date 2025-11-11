using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public abstract class BaseHandler : IPageItemHandler
{
    protected readonly NorthwindContext Db;
    protected BaseHandler(NorthwindContext db) => Db = db;

    public abstract string Type { get; }

    public virtual Task<object?> ExecuteParameterAsync(Dictionary<string, object> args, ParameterStore store, CancellationToken ct)
        => ExecuteItemAsync(args, store, ct); // default: samma logik

    public abstract Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct);

    protected int GetTopArg(Dictionary<string, object> dict, int defaultValue = 5)
        => dict.TryGetValue("top", out var v) && int.TryParse(Convert.ToString(v), out var n) ? n : defaultValue;

    // 🔹 Filtrering efter roll/supplier/employee
    protected IQueryable<Order> FilterOrders(IQueryable<Order> query, ParameterStore ps)
    {
        var roles = ps.Get("userRoles")?.ToString()?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];
        var supplierId = ps.Get("supplierId")?.ToString();
        var employeeId = ps.Get("employeeId")?.ToString();

        if (roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
            return query;

        if (!string.IsNullOrEmpty(supplierId))
            return query.Where(o => o.OrderDetails.Any(od => od.Product.SupplierId.ToString() == supplierId));

        if (!string.IsNullOrEmpty(employeeId))
            return query.Where(o => o.EmployeeId.ToString() == employeeId);

        return query;
    }

    // 🔹 En konsekvent avrundningshjälpare
    protected static decimal Round(decimal value, int decimals = 1)
        => Math.Round(value, decimals);
}
