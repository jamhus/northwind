using Microsoft.EntityFrameworkCore;
using Northwind.Dashboard.Engine;
using Northwind.Models;
using Northwind.Models.Data;

namespace Northwind.Dashboard.Handlers;

public class TopCustomersHandler : BaseHandler
{
    public override string Type => "TopCustomers";
    public TopCustomersHandler(NorthwindContext db) : base(db) { }

    public override async Task<object?> ExecuteItemAsync(Dictionary<string, object> settings, ParameterStore ps, CancellationToken ct)
    {
        var top = GetTopArg(settings, 5);
        var query = FilterOrders(Db.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails), ps);

        var topCustomers = await query
            .Where(o => o.Customer != null)
            .GroupBy(o => o.Customer.CompanyName)
            .Select(g => new
            {
                Customer = g.Key,
                TotalSales = g.SelectMany(o => o.OrderDetails)
                              .Sum(od => od.Quantity * od.UnitPrice)
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(top)
            .ToListAsync(ct);

        return topCustomers.Select(x => new
        {
            x.Customer,
            TotalSales = Round(x.TotalSales, 1)
        });
    }
}
