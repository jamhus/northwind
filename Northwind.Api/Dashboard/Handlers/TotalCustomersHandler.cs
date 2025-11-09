using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TotalCustomersHandler : BaseHandler
{
    public TotalCustomersHandler(NorthwindContext db) : base(db) { }
    public override string Type => "TotalCustomers";

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var count = await Db.Customers.CountAsync(ct);
        return new { totalCustomers = count };
    }
}
