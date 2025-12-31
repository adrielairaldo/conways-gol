# CORS Configuration

This configuration allows you to establish Cross-Origin Resource Sharing (CORS) rules for your ASP.NET Core application.

1. In `Program.cs`, extend the application builder as follows:

    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    app.UseCustomCors(builder.Configuration);

    app.Run();
    ```

    This will add the functionality that sets the CORS rules using the `CorsSettings` configuration settings.

2. Add the configuration data in `appsettings.json` (or your preferred configuration source) as follows:

    ```json
    {
        "CorsSettings": {
            "AllowedOrigins": [
                "https://example.com",
                "https://anotherexample.com"
            ]
        }
    }
    ```

    The `AllowedOrigins` parameter specifies the origins that are allowed to access your application. This should be a list of strings representing the allowed origins.

With these parameters, the `UseCustomCors` extension method will configure the CORS settings accordingly, allowing specified origins to access your application.

### Example `appsettings.json`

Here is an example of how to configure the `CorsSettings` in your `appsettings.json` file:

```json
{
    "CorsSettings": {
        "AllowedOrigins": [
            "https://example.com",
            "https://anotherexample.com"
        ]
    }
}