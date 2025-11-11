using System.Text.RegularExpressions;
using Northwind.Dashboard.Models;
using Northwind.Dashboard.Handlers;

namespace Northwind.Dashboard.Engine;

public class ParameterEvaluator
{
    private readonly IEnumerable<IPageItemHandler> _handlers;
    private readonly DynamicDataService _dynamic;

    public ParameterEvaluator(IEnumerable<IPageItemHandler> handlers, DynamicDataService dynamic)
    {
        _handlers = handlers;
        _dynamic = dynamic;
    }

    public async Task<ParameterStore> InitializeAsync(IEnumerable<ParameterDefinition> defs, CancellationToken ct)
    {
        var store = new ParameterStore();

        foreach (var p in defs)
        {
            switch (p.Initialization)
            {
                case ParameterInitialization.Explicit:
                    store.Set(p.Key, p.Value);
                    break;

                case ParameterInitialization.Expression:
                    {
                        var value = await EvaluateDynamicExpressionAsync(p.Expression);
                        store.Set(p.Key, value);
                        break;
                    }


                case ParameterInitialization.Handler:
                    // Format: "Handler.TopProducts(5)" eller "Handler.TotalSales()"
                    if (TryParseHandlerCall(p.Expression, out var type, out var args))
                    {
                        var handler = _handlers.FirstOrDefault(h =>
                            h.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
                        if (handler != null)
                        {
                            var result = await handler.ExecuteParameterAsync(args, store, ct);
                            store.Set(p.Key, result);
                        }
                        else store.Set(p.Key, null);
                    }
                    else store.Set(p.Key, null);
                    break;
            }
        }

        return store;
    }

    static bool TryParseHandlerCall(string? expr, out string type, out Dictionary<string, object> args)
    {
        type = "";
        args = new();

        if (string.IsNullOrWhiteSpace(expr) || !expr.StartsWith("Handler.", StringComparison.OrdinalIgnoreCase))
            return false;

        // Ex: "Handler.TopProducts(5)" -> type=TopProducts, args={ top: 5 }
        var m = Regex.Match(expr, @"Handler\.(?<type>\w+)\((?<arg>[^)]*)\)");
        if (!m.Success) return false;

        type = m.Groups["type"].Value;
        var arg = m.Groups["arg"].Value.Trim();

        if (string.IsNullOrEmpty(arg)) return true;

        // enkel tolkning: ett heltal tolkas som { top: <int> }
        if (int.TryParse(arg, out var n)) args["top"] = n;
        // (utöka här om du vill stödja "key:value, key:value" etc.)

        return true;
    }

    private async Task<object?> EvaluateDynamicExpressionAsync(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return null;

        // Förväntat format: DynamicData.methodName(args)
        if (!expression.StartsWith("DynamicData.", StringComparison.OrdinalIgnoreCase))
            return null;

        var match = Regex.Match(expression, @"DynamicData\.(?<method>\w+)\((?<args>[^)]*)\)");
        if (!match.Success)
            return null;

        var methodName = match.Groups["method"].Value;
        var args = match.Groups["args"].Value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();

        // Leta upp metoden på DynamicDataService
        var method = typeof(DynamicDataService)
            .GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);

        if (method == null)
            return null;

        var parameters = method.GetParameters();
        object?[] parsedArgs = new object?[parameters.Length];

        // Just nu stödjer vi bara primitiva typer (int, string, bool)
        for (int i = 0; i < parameters.Length; i++)
        {
            if (i >= args.Length)
                break;

            try
            {
                parsedArgs[i] = Convert.ChangeType(args[i], parameters[i].ParameterType);
            }
            catch
            {
                parsedArgs[i] = null;
            }
        }

        var result = method.Invoke(_dynamic, parsedArgs);

        // Om metoden returnerar Task
        if (result is Task task)
        {
            await task.ConfigureAwait(false);
            var resultProp = task.GetType().GetProperty("Result");
            return resultProp?.GetValue(task);
        }

        return result;
    }

}
