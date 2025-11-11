using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Northwind.Dashboard.Engine;
using System.Text.Json;
using System.Security.Claims;

namespace Northwind.Endpoints.Dashboard;

public class PreviewDashboardConfig : EndpointBaseAsync
    .WithRequest<JsonElement>
    .WithActionResult<object>
{
    private readonly DashboardRuntimeService _runtime;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PreviewDashboardConfig(
        DashboardRuntimeService runtime,
        IHttpContextAccessor httpContextAccessor)
    {
        _runtime = runtime;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("api/dashboardConfig/preview", Name = nameof(PreviewDashboardConfig))]
    public override async Task<ActionResult<object>> HandleAsync(
        [FromBody] JsonElement config,
        CancellationToken ct = default)
    {
        try
        {
            // Konvertera JsonElement → ren text
            var json = config.GetRawText();

            // Hämta användaren (admin eller annan roll)
            var user = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

            // Kör igenom dashboardmotorn utan att spara något
            var result = await _runtime.RenderAsync(json, user, ct);

            return Ok(result); // innehåller Pages, Items, Data, etc.
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "Could not render dashboard preview.",
                error = ex.Message
            });
        }
    }
}
