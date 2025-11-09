using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public abstract class BaseHandler : IReportItemHandler
{
    protected readonly NorthwindContext Db;
    protected BaseHandler(NorthwindContext db) => Db = db;

    public abstract string Type { get; }

    public virtual Task<object?> ExecuteParameterAsync(Dictionary<string, object> args, ParameterStore store, CancellationToken ct)
        => ExecuteItemAsync(args, store, ct); // default: samma logik

    public abstract Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct);

    protected int GetTopArg(Dictionary<string, object> dict, int defaultValue = 5)
        => dict.TryGetValue("top", out var v) && int.TryParse(Convert.ToString(v), out var n) ? n : defaultValue;
}
