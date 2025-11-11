using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TotalOrdersHandler : BaseHandler
{
    public override string Type => "TotalOrders";
    public TotalOrdersHandler(NorthwindContext db) : base(db) { }

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore ps, CancellationToken ct)
    {
        var count = await FilterOrders(Db.Orders, ps).CountAsync(ct);
        return count;
    }
}
