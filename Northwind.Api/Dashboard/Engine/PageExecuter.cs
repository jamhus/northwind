using Northwind.Dashboard.Models;
using Northwind.Dashboard.Handlers;

namespace Northwind.Dashboard.Engine;

public class PageExecutor
{
    private readonly IEnumerable<IPageItemHandler> _handlers;
    private readonly ConditionEvaluator _conditions;

    public PageExecutor(IEnumerable<IPageItemHandler> handlers, ConditionEvaluator conditions)
    {
        _handlers = handlers;
        _conditions = conditions;
    }

    public async Task<List<RenderedPage>> ExecuteAsync(
        IEnumerable<Page> pages,
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
                Layout = new Layout
                {
                    Type = page.Layout.Type,
                    Rows = new List<LayoutRow>()
                }
            };

            var visibleItems = new List<RenderedItem>();

            // 🔹 1. Kör igenom alla items separat först (för condition-filtering)
            foreach (var item in page.PageItems)
            {
                if (!_conditions.Evaluate(item.Condition, store, claimAccessor))
                    continue;

                var handler = _handlers.FirstOrDefault(h =>
                    h.Type.Equals(item.PageItemType, StringComparison.OrdinalIgnoreCase));

                if (handler is null)
                    continue;

                var data = await handler.ExecuteItemAsync(item.Settings ?? new(), store, ct);

                visibleItems.Add(new RenderedItem
                {
                    Key = item.Key,
                    Type = item.PageItemType,
                    Settings = item.Settings,
                    Data = data
                });
            }

            // 🔹 2. Skapa en lookup så vi vet vilka items som ska synas
            var visibleKeys = visibleItems.Select(i => i.Key).ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 🔹 3. Filtrera layouten baserat på vilka items som finns
            rp.Layout.Rows = page.Layout.Rows
                .Select(row => new LayoutRow
                {
                    Columns = row.Columns
                        .Where(col => !string.IsNullOrWhiteSpace(col.ItemRef)
                                      && visibleKeys.Contains(col.ItemRef))
                        .ToList(),
                    Style = row.Style
                })
                .Where(row => row.Columns.Count > 0)
                .ToList();

            // 🔹 4. Lägg till de synliga itemsen
            rp.PageItems.AddRange(visibleItems);

            result.Add(rp);
        }

        return result;
    }
}
