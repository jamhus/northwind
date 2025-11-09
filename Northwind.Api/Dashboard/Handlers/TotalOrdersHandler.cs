using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TotalOrdersHandler : BaseHandler
{
    public TotalOrdersHandler(NorthwindContext db) : base(db) { }
    public override string Type => "TotalOrders";

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var count = await Db.Orders.CountAsync(ct);
        return new { totalOrders = count };
    }
}
