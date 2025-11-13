using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class AvgOrderProcessingTimeHandler : BaseHandler
{
    public AvgOrderProcessingTimeHandler(NorthwindContext db) : base(db) { }

    public override string Type => "AvgOrderProcessingTime";

    public override async Task<object?> ExecuteItemAsync(
        Dictionary<string, object> settings,
        ParameterStore store,
        CancellationToken ct)
    {
        var avgDays = await Db.Orders
            .Where(o => o.OrderDate != null && o.ShippedDate != null)
            .AverageAsync(o => EF.Functions.DateDiffDay(o.OrderDate, o.ShippedDate), ct);

        return Math.Round((decimal)avgDays, 1);
    }
}
