using System;
using Microsoft.Extensions.Configuration;

namespace Conways.Service.HttpApi.Extensions.CorsSettings;

/// <summary>
/// Extension methods for retrieving service settings from IConfiguration.
/// </summary>
public static class CorsSettingsExtensions
{
    /// <summary>
    /// Gets the <see cref="CorsSettings"/> from the specified <see cref="IConfiguration"/> instance, or throws an exception if it's null.
    /// </summary>
    /// <param name="configuration">The configuration instance.</param>
    /// <returns>The <see cref="CorsSettings"/> object.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="CorsSettings"/> is not found in the configuration.
    /// </exception>
    public static CorsSettings GetCorsSettings(this IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

        if (corsSettings == null)
        {
            throw new InvalidOperationException($"{nameof(CorsSettings)} is not found in the configuration. Make sure it's properly configured.");
        }

        return corsSettings;
    }
}