using System.Text.Json.Serialization;

namespace Northwind.Dashboard.Models;

public class DashboardDefinition
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("companyId")]
    public int CompanyId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "Dashboard";

    [JsonPropertyName("parameters")]
    public List<ParameterDefinition> Parameters { get; set; } = [];

    [JsonPropertyName("pages")]
    public List<Page> Pages { get; set; } = [];
}

public class ParameterDefinition
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("initialization")]
    public ParameterInitialization Initialization { get; set; }

    [JsonPropertyName("entityType")]
    public string? EntityType { get; set; }

    [JsonPropertyName("expression")]
    public string? Expression { get; set; }

    [JsonPropertyName("value")]
    public object? Value { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public class Page
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = "";

    [JsonPropertyName("name")]
    public List<LocalizedName> Name { get; set; } = [];

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonPropertyName("layout")]
    public Layout Layout { get; set; } = new();

    [JsonPropertyName("pageItems")]
    public List<PageItem> PageItems { get; set; } = [];
    [JsonPropertyName("condition")]
    public ConditionDto? Condition { get; set; }
}

public class LocalizedName
{
    [JsonPropertyName("language")]
    public string Language { get; set; } = "sv";

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
}

public class Layout
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "vertical";  // Vertical | Horizontal | Grid

    [JsonPropertyName("rows")]
    public List<LayoutRow> Rows { get; set; } = [];
}

public class LayoutRow
{
    [JsonPropertyName("columns")]
    public List<LayoutColumn> Columns { get; set; } = [];

    [JsonPropertyName("style")]
    public Dictionary<string, object>? Style { get; set; }
}

public class LayoutColumn
{
    [JsonPropertyName("itemRef")]
    public string? ItemRef { get; set; } // refererar till PageItems[key]

    [JsonPropertyName("style")]
    public Dictionary<string, object>? Style { get; set; }
}

public class PageItem
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = "";

    [JsonPropertyName("pageItemType")]
    public string PageItemType { get; set; } = ""; // matchar handler.Type

    [JsonPropertyName("settings")]
    public Dictionary<string, object>? Settings { get; set; }

    [JsonPropertyName("condition")]
    public ConditionDto? Condition { get; set; }
}

public class ConditionDto
{
    [JsonPropertyName("logic")]
    public string Logic { get; set; } = "And"; // And | Or

    [JsonPropertyName("conditions")]
    public List<ConditionDto>? Conditions { get; set; }

    [JsonPropertyName("operator")]
    public string? Operator { get; set; }      // Eq, Ne, Gt, Lt, Gte, Lte, Contains

    [JsonPropertyName("leftSource")]
    public string? LeftSource { get; set; }    // Parameter | Const | Claim

    [JsonPropertyName("leftField")]
    public string? LeftField { get; set; }

    [JsonPropertyName("rightSource")]
    public string? RightSource { get; set; }   // Parameter | Const | Claim

    [JsonPropertyName("rightField")]
    public string? RightField { get; set; }

    [JsonPropertyName("rightValue")]
    public string? RightValue { get; set; }
}
