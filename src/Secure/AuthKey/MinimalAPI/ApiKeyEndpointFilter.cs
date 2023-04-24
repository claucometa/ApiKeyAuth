using ApiKeyAuthentication.Secure.MinimalAPI;

namespace ApiKeyAuthentication.Secure.Key.MinimalAPI
{
    public class ApiKeyEndpointFilter : IEndpointFilter
    {
        private readonly IConfiguration config;

        public ApiKeyEndpointFilter(IConfiguration config)
        {
            this.config = config;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyConfig.ApiKey, out var apiKeyVal))
            {
                //return TypedResults.Unauthorized();
                return new ApiKeyResultHandler("API Key missing");
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = appSettings.GetValue<string>(ApiKeyConfig.ApiKey);
            if (apiKey == null || !apiKey.Equals(apiKeyVal))
            {
                return new ApiKeyResultHandler("Invalid API key");
            }

            return await next(context);
        }
    }
}
