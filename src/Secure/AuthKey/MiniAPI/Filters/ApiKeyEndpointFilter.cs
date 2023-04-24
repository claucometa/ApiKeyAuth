using ApiKeyAuth.Models;

namespace ApiKeyAuth.Secure.AuthKey.MinimalAPI.Filter
{
    public class ApiKeyEndpointFilter : IEndpointFilter
    {
        private readonly IConfiguration _config;

        public ApiKeyEndpointFilter(IConfiguration config)
        {
            _config = config;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyConfig.ApiKey, out var apiKeyVal))
            {
                //return TypedResults.Unauthorized();
                return new FriendlyErrorMessage("API Key missing");
            }

            var apiKey = _config.GetValue<string>(ApiKeyConfig.ApiKey);
            if (apiKey == null || !apiKey.Equals(apiKeyVal))
            {
                return new FriendlyErrorMessage("Invalid API key");
            }

            return await next(context);
        }

        class FriendlyErrorMessage : IResult, IStatusCodeHttpResult
        {
            readonly object _body;

            public FriendlyErrorMessage(object body)
            {
                _body = body;
            }

            public int? StatusCode => StatusCodes.Status401Unauthorized;

            public async Task ExecuteAsync(HttpContext httpContext)
            {
                ArgumentNullException.ThrowIfNull(httpContext);

                if (StatusCode.HasValue)
                    httpContext.Response.StatusCode = StatusCode.Value;

                if (_body is string s)
                    await httpContext.Response.WriteAsync(s);
                else
                    await httpContext.Response.WriteAsJsonAsync(_body);
            }
        }
    }
}
