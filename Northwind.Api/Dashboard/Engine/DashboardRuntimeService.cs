using System.Text.Json;
using System.Security.Claims;
using Northwind.Dashboard.Models;

namespace Northwind.Dashboard.Engine;

public class DashboardRuntimeService
{
    private readonly ParameterEvaluator _parameters;
    private readonly ReportPageExecutor _executor;
    private readonly DynamicDataService _dynamic;

    public DashboardRuntimeService(ParameterEvaluator parameters, ReportPageExecutor executor, DynamicDataService dynamic)
    {
        _parameters = parameters;
        _executor = executor;
        _dynamic = dynamic;
    }

    public async Task<RenderedDashboardResult> RenderAsync(string configJson, ClaimsPrincipal user, CancellationToken ct = default)
    {
        var def = JsonSerializer.Deserialize<DashboardDefinition>(configJson)
                  ?? throw new InvalidOperationException("Invalid dashboard JSON");

        var store = await _parameters.InitializeAsync(def.Parameters, ct);

        string? ClaimAccessor(string type) => user.FindFirstValue(type);

        var pages = await _executor.ExecuteAsync(def.ReportPages, store, ClaimAccessor, ct);
        return new RenderedDashboardResult { Pages = pages };
    }
}
