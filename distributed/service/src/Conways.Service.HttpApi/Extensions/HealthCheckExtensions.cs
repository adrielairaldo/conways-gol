using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Conways.Service.HttpApi.Extensions;

public static class HealthCheckExtensions
{
    /// <summary>
    /// Add the readyness endpoint at /health/ready, running all previously registered health checks.
    /// </summary>
    /// <param name="endpoints">The Microsoft.AspNetCore.Routing.IEndpointRouteBuilder to add the health checks endpoint to.</param>
    public static void UseReadynessHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health/ready",new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    /// <summary>
    /// Add the liveness endpoint at /health/live. It does not run any previously declared health
    /// checks, so it will respond Healthy whenever the app is running, regardless of the state
    /// of the other services.
    /// </summary>
    /// <param name="endpoints">The Microsoft.AspNetCore.Routing.IEndpointRouteBuilder to add the health checks endpoint to.</param>
    public static void UseLivenessHealthCheck(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false, // Here we avoid running all registered HealthChecks (this means that will respond Healthy only if the app is running).
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}