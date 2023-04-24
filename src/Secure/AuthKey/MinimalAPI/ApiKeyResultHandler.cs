namespace ApiKeyAuthentication.Secure.Key.MinimalAPI
{
    public class ApiKeyResultHandler : IResult, IStatusCodeHttpResult
    {
        readonly object _body;

        public ApiKeyResultHandler(object body)
        {
            _body = body;
        }

        public int? StatusCode => StatusCodes.Status401Unauthorized;

        int? IStatusCodeHttpResult.StatusCode => StatusCode;


        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            if (StatusCode.HasValue)
                httpContext.Response.StatusCode = StatusCode.Value;

            if (_body is string s)
            {
                await httpContext.Response.WriteAsync(s);
                return;
            }

            await httpContext.Response.WriteAsJsonAsync(_body);
        }
    }
}
