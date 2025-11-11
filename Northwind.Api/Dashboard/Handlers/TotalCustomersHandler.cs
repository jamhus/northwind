using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TotalCustomersHandler : BaseHandler
{
    public override string Type => "TotalCustomers";
    public TotalCustomersHandler(NorthwindContext db) : base(db) { }

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore ps, CancellationToken ct)
    {
        var totalCustomers = await FilterOrders(Db.Orders.Include(o => o.Customer), ps)
            .Select(o => o.CustomerId)
            .Distinct()
            .CountAsync(ct);

        return totalCustomers;
    }
}
