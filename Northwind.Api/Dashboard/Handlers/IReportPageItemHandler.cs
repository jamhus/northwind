using Northwind.Dashboard.Engine;

namespace Northwind.Dashboard.Handlers;

public interface IPageItemHandler
{
    string Type { get; } // ex: "TotalSales", "TopProducts"
    Task<object?> ExecuteParameterAsync(Dictionary<string, object> args, ParameterStore store, CancellationToken ct);
    Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct);
}
