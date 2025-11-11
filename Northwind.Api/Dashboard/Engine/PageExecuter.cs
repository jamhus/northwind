using Northwind.Dashboard.Models;
using Northwind.Dashboard.Handlers;

namespace Northwind.Dashboard.Engine;

public class PageExecutor
{
    private readonly IEnumerable<IReportItemHandler> _handlers;
    private readonly ConditionEvaluator _conditions;

    public PageExecutor(IEnumerable<IReportItemHandler> handlers, ConditionEvaluator conditions)
    {
        _handlers = handlers;
        _conditions = conditions;
    }

    public async Task<List<RenderedPage>> ExecuteAsync(IEnumerable<Page> pages,
        ParameterStore store,
        Func<string, string?> claimAccessor,
        CancellationToken ct)
    {
        var result = new List<RenderedPage>();

        foreach (var page in pages.Where(p => p.Enabled))
        {
            var rp = new RenderedPage
            {
                Key = page.Key,
                Title = page.Name.FirstOrDefault()?.Text ?? page.Key,
                Name = page.Name,
                Layout = page.Layout,
            };

            // layouten pekar på items via itemRef
            foreach (var row in page.Layout.Rows)
            {
                foreach (var col in row.Columns)
                {
                    if (string.IsNullOrWhiteSpace(col.ItemRef)) continue;
                    var item = page.PageItems.FirstOrDefault(i => i.Key == col.ItemRef);
                    if (item is null) continue;

                    if (!_conditions.Evaluate(item.Condition, store, claimAccessor))
                        continue;

                    var handler = _handlers.FirstOrDefault(h =>
                        h.Type.Equals(item.PageItemType, StringComparison.OrdinalIgnoreCase));

                    if (handler is null) continue;

                    var data = await handler.ExecuteItemAsync(item.Settings ?? new(), store, ct);
                    rp.PageItems.Add(new RenderedItem
                    {
                        Key = item.Key,
                        Type = item.PageItemType,
                        Settings = item.Settings,
                        Data = data
                    });
                }
            }

            result.Add(rp);
        }

        return result;
    }
}
