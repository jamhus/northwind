using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TotalSalesHandler : BaseHandler
{
    public override string Type => "TotalSales";
    public TotalSalesHandler(NorthwindContext db) : base(db) { }

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore ps, CancellationToken ct)
    {
        var query = FilterOrders(Db.Orders.Include(o => o.OrderDetails), ps);

        var totalSales = await query
            .SelectMany(o => o.OrderDetails)
            .SumAsync(od => od.Quantity * od.UnitPrice, ct);

        return Round(totalSales, 1);
    }
}
