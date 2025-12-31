using System;

namespace Conways.Service.HttpApi.Extensions.CorsSettings;

/// <summary>
/// Represents the CORS settings.
/// </summary>
public class CorsSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CorsSettings"/> class.
    /// </summary>
    /// <param name="allowedOrigins">An array of allowed origins for CORS.</param>
    /// <exception cref="ArgumentNullException">Thrown when allowedOrigins is null or empty.</exception>
    public CorsSettings(string[]? allowedOrigins)
    {
        if (allowedOrigins == null || allowedOrigins.Length == 0)
        {
            throw new ArgumentNullException(nameof(allowedOrigins), $"{nameof(allowedOrigins)} cannot be null or empty.");
        }

        this.AllowedOrigins = allowedOrigins;
    }

    /// <summary>
    /// Gets the array of allowed origins.
    /// </summary>
    public string[] AllowedOrigins { get; }
}