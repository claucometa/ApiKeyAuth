using ApiKeyAuth.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiKeyAuthentication.Secure.Key.RestAPI.Strategy.Attributes
{
    /// <summary>
    /// This does the same as ApiKeyMiddleware but using attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyConfig.ApiKey, out var apiKeyVal))
            {
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("Api Key not found!");
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(ApiKeyConfig.ApiKey);
            if (!apiKey.Equals(apiKeyVal))
            {
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("Unauthorized client");
            }

        }
    }
}
