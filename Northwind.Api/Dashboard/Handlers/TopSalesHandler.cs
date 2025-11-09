using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TotalSalesHandler : BaseHandler
{
    public TotalSalesHandler(NorthwindContext db) : base(db) { }
    public override string Type => "TotalSales";

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore store, CancellationToken ct)
    {
        var total = await Db.OrderDetails.SumAsync(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount), ct);
        return new { totalSales = total };
    }
}
