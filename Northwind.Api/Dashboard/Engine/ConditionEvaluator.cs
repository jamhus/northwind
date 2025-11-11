using Northwind.Dashboard.Models;
using System.Globalization;

namespace Northwind.Dashboard.Engine;

public class ConditionEvaluator
{
    public bool Evaluate(ConditionDto? c, ParameterStore ps, Func<string, string?> claimAccessor)
    {
        if (c == null) return true;

        // Hantera logiska grupper (And / Or)
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

    // ------------------------------------------------------------------------

    private static object? Resolve(string? src, string? field, string? constVal, ParameterStore ps, Func<string, string?> claim)
    {
        if (string.Equals(src, "Parameter", StringComparison.OrdinalIgnoreCase))
            return field != null ? ps.Get(field) : null;

        if (string.Equals(src, "Claim", StringComparison.OrdinalIgnoreCase))
            return field != null ? claim(field) : null;

        if (string.Equals(src, "Const", StringComparison.OrdinalIgnoreCase))
            return constVal;

        return null;
    }

    // ------------------------------------------------------------------------

    private static bool Compare(object? l, object? r, string? op)
    {
        if (string.IsNullOrWhiteSpace(op))
            return true;

        string sl = Convert.ToString(l) ?? "";
        string sr = Convert.ToString(r) ?? "";

        // Numeriska jämförelser (GT, LT, Gte, Lte)
        bool TryCompareNumeric(out int result)
        {
            result = 0;
            if (double.TryParse(sl, NumberStyles.Any, CultureInfo.InvariantCulture, out var dl) &&
                double.TryParse(sr, NumberStyles.Any, CultureInfo.InvariantCulture, out var dr))
            {
                result = dl.CompareTo(dr);
                return true;
            }
            return false;
        }

        switch (op)
        {
            case "Eq":
                return string.Equals(sl, sr, StringComparison.OrdinalIgnoreCase);

            case "Neq":
                return !string.Equals(sl, sr, StringComparison.OrdinalIgnoreCase);

            case "Gt":
                return TryCompareNumeric(out var gt) && gt > 0;

            case "Lt":
                return TryCompareNumeric(out var lt) && lt < 0;

            case "Gte":
                return TryCompareNumeric(out var gte) && gte >= 0;

            case "Lte":
                return TryCompareNumeric(out var lte) && lte <= 0;

            // 🔹 Nytt: hantera listor av värden
            case "Contains":
                return ContainsValue(sl, sr);

            case "NotContains":
                return !ContainsValue(sl, sr);

            case "In":
                return IsInList(sl, sr);

            default:
                return true;
        }
    }

    // ------------------------------------------------------------------------

    private static bool ContainsValue(string left, string right)
    {
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
            return false;

        var parts = left.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Any(p => string.Equals(p, right, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsInList(string left, string rightList)
    {
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(rightList))
            return false;

        var list = rightList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return list.Contains(left, StringComparer.OrdinalIgnoreCase);
    }
}
