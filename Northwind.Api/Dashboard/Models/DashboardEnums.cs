namespace Northwind.Dashboard.Models
{
    public enum ParameterInitialization
    {
        Expression,   // "DynamicData.getIsAdmin()"
        Handler,      // "Handler.TopProducts(5)"
        Explicit      // direkt value i JSON
    }

    public enum LayoutType { Vertical, Horizontal, Grid }

}
