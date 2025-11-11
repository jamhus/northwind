using Northwind.Dashboard.Models;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Northwind.Dashboard.Engine;

public class DashboardRuntimeService
{
    private readonly ParameterEvaluator _parameters;
    private readonly PageExecutor _executor;
    private readonly DynamicDataService _dynamic;

    public DashboardRuntimeService(ParameterEvaluator parameters, PageExecutor executor, DynamicDataService dynamic)
    {
        _parameters = parameters;
        _executor = executor;
        _dynamic = dynamic;
    }

    public async Task<RenderedDashboardResult> RenderAsync(string configJson, ClaimsPrincipal user, CancellationToken ct = default)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        var def = JsonSerializer.Deserialize<DashboardDefinition>(configJson, options)
                  ?? throw new InvalidOperationException("Invalid dashboard JSON");

        var store = await _parameters.InitializeAsync(def.Parameters, ct);

        string? ClaimAccessor(string type) => user.FindFirstValue(type);

        var pages = await _executor.ExecuteAsync(def.Pages, store, ClaimAccessor, ct);
        return new RenderedDashboardResult { Pages = pages };
    }
}
