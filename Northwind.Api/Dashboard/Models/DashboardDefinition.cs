using System.Text.Json.Serialization;

namespace Northwind.Dashboard.Models;

public class DashboardDefinition
{
    [JsonPropertyName("version")] public int Version { get; set; }
    [JsonPropertyName("companyId")] public int CompanyId { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = "Dashboard";
    [JsonPropertyName("parameters")] public List<ParameterDefinition> Parameters { get; set; } = [];
    [JsonPropertyName("reportPages")] public List<ReportPage> ReportPages { get; set; } = [];
}

public class ParameterDefinition
{
    public string Key { get; set; } = "";
    public ParameterInitialization Initialization { get; set; }
    public string? EntityType { get; set; }
    public string? Expression { get; set; }   // t.ex. "Handler.TopProducts(5)" eller "DynamicData.getIsAdmin()"
    public string? Description { get; set; }
    public string? Expiration { get; set; }   // "1.00:00:00" (lagras ej här, men bra för framtiden)
    public object? Value { get; set; }        // om Explicit
}

public class ReportPage
{
    public string Key { get; set; } = "";
    public List<LocalizedName> Name { get; set; } = [];
    public bool Enabled { get; set; } = true;
    public Layout Layout { get; set; } = new();
    public List<ReportPageItem> ReportPageItems { get; set; } = [];
}

public class LocalizedName
{
    public string Language { get; set; } = "sv";
    public string Text { get; set; } = "";
}

public class Layout
{
    public string Type { get; set; } = "vertical";  // Vertical | Horizontal | Grid
    public List<LayoutRow> Rows { get; set; } = [];
}

public class LayoutRow
{
    public List<LayoutColumn> Columns { get; set; } = [];
    public Dictionary<string, object>? Style { get; set; }
}

public class LayoutColumn
{
    public string? ItemRef { get; set; } // refererar till ReportPageItems[key]
    public Dictionary<string, object>? Style { get; set; }
}

public class ReportPageItem
{
    public string Key { get; set; } = "";
    public string ReportPageItemType { get; set; } = ""; // matchar handler.Type
    public Dictionary<string, object>? Settings { get; set; }
    public ConditionDto? Condition { get; set; }
}

public class ConditionDto
{
    public string Logic { get; set; } = "And"; // And | Or
    public List<ConditionDto>? Conditions { get; set; }

    public string? Operator { get; set; }      // Eq, Ne, Gt, Lt, Gte, Lte
    public string? LeftSource { get; set; }    // Parameter | Const | Claim
    public string? LeftField { get; set; }
    public string? RightSource { get; set; }   // Parameter | Const | Claim
    public string? RightField { get; set; }
    public string? RightValue { get; set; }
}
