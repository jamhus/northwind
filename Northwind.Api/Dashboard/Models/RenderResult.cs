namespace Northwind.Dashboard.Models;

public class RenderedDashboardResult
{
    public List<RenderedPage> Pages { get; set; } = [];
}

public class RenderedPage
{
    public string Key { get; set; } = "";
    public string Title { get; set; } = "";
    public List<LocalizedName> Name { get; set; }
    public List<RenderedItem> PageItems { get; set; } = [];
    public Layout? Layout { get; set; }
}


public class RenderedItem
{
    public string Key { get; set; } = "";
    public string Type { get; set; } = ""; // PageItemType (t.ex. "TopProducts")
    public Dictionary<string, object>? Settings { get; set; }
    public object? Data { get; set; }
}
