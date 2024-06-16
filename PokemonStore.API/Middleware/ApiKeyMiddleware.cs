namespace PokemonStore.API.Middleware;
public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private const string APIKEYNAME = "ApiKey";
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<ApiKeyMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            _logger.LogWarning("API Key was not provided.");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var apiKey = _configuration.GetValue<string>($"ApiKeySettings:{APIKEYNAME}");

        if (!apiKey.Equals(extractedApiKey))
        {
            _logger.LogWarning("Unauthorized client.");
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}

