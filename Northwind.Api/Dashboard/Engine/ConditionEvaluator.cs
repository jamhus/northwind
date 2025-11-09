using Northwind.Dashboard.Models;

namespace Northwind.Dashboard.Engine;

public class ConditionEvaluator
{
    public bool Evaluate(ConditionDto? c, ParameterStore ps, Func<string, string?> claimAccessor)
    {
        if (c == null) return true;
        // If there are nested conditions, evaluate them based on the logic (And/Or)
        if (c.Conditions is { Count: > 0 })
        {
            var results = c.Conditions.Select(x => Evaluate(x, ps, claimAccessor));
            return c.Logic.Equals("Or", StringComparison.OrdinalIgnoreCase)
                ? results.Any(r => r)
                : results.All(r => r);
        }

        var left = Resolve(c.LeftSource, c.LeftField, c.RightValue, ps, claimAccessor);
        var right = Resolve(c.RightSource, c.RightField, c.RightValue, ps, claimAccessor);
        return Compare(left, right, c.Operator);
    }

    // Resolves the value based on the source type
    // if src is "Parameter", it fetches from ParameterStore using field as key
    // Example-left: Resolve("Parameter", "isAdmin", 1, ps, claimAccessor) => ps.Get("isAdmin") 1 or 0
    // Example-right: Resolve("Const", null, "1", ps, claimAccessor) => "1"

    static object? Resolve(string? src, string? field, string? constVal, ParameterStore ps, Func<string, string?> claim)
    {
        if (string.Equals(src, "Parameter", StringComparison.OrdinalIgnoreCase))
            return field != null ? ps.Get(field) : null;

        if (string.Equals(src, "Claim", StringComparison.OrdinalIgnoreCase))
            return field != null ? claim(field) : null;

        if (string.Equals(src, "Const", StringComparison.OrdinalIgnoreCase))
            return constVal;

        return null;
    }
    // Compares two objects based on the operator
    // Supports Eq, Ne, Gt, Lt, Gte, Lte
    // Example: Compare(5, 3, "Gt") => true
    static bool Compare(object? l, object? r, string? op)
    {
        if (op is null) return true;

        string sl = Convert.ToString(l) ?? "";
        string sr = Convert.ToString(r) ?? "";

        return op switch
        {
            "Eq" => string.Equals(sl, sr, StringComparison.OrdinalIgnoreCase),
            "Ne" => !string.Equals(sl, sr, StringComparison.OrdinalIgnoreCase),
            "Gt" => ToDecimal(sl) > ToDecimal(sr),
            "Lt" => ToDecimal(sl) < ToDecimal(sr),
            "Gte" => ToDecimal(sl) >= ToDecimal(sr),
            "Lte" => ToDecimal(sl) <= ToDecimal(sr),
            _ => true
        };
    }

    static decimal ToDecimal(string s) => decimal.TryParse(s, out var d) ? d : 0m;
}
