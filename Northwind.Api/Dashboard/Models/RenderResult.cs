namespace Northwind.Dashboard.Models;

public class RenderedDashboardResult
{
    public List<RenderedPage> Pages { get; set; } = [];
}

public class RenderedPage
{
    public string Key { get; set; } = "";
    public string Title { get; set; } = "";
    public List<RenderedItem> Items { get; set; } = [];
}

public class RenderedItem
{
    public string Key { get; set; } = "";
    public string Type { get; set; } = ""; // ReportPageItemType (t.ex. "TopProducts")
    public Dictionary<string, object>? Settings { get; set; }
    public object? Data { get; set; }
}
