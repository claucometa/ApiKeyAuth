using ApiKeyAuth.Models;

namespace ApiKeyAuthentication.Secure.Key.RestAPI.Strategy.Middlewares
{
    /// <summary>
    /// This does the same as ApiKeyAttribute but using MiddleWhare
    /// </summary>
    public class ApiKeyMiddleware
    {
        readonly RequestDelegate _requestDelegate;        

        public ApiKeyMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyConfig.ApiKey, out var apiKeyVal))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key not found!");
            }

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(ApiKeyConfig.ApiKey);
            if (apiKey == null || !apiKey.Equals(apiKeyVal))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client");
            }

            await _requestDelegate(context);
        }
    }
}
