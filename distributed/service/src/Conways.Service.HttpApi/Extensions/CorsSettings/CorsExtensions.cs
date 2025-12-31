namespace Conways.Service.HttpApi.Extensions.CorsSettings;

/// <summary>
/// Extension methods for configuring CORS in an ASP.NET Core application.
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Configures the application to use custom CORS settings defined in the configuration.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="configuration">The configuration instance containing CORS settings.</param>
    public static void UseCustomCors(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseCors(corsBuilder =>
        {
            var corsSettings = configuration.GetCorsSettings();

            corsBuilder.WithOrigins(corsSettings.AllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Just for SignalR
        });
    }
}