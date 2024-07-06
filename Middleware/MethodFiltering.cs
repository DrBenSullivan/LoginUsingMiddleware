namespace LoginUsingMiddleware.Middleware
{
    public class MethodFiltering
    {
        private readonly RequestDelegate _next;

        public MethodFiltering(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Conditional middleware for HTTP GET request: 200 Empty Response.
            if (httpContext.Request.Path == "/" && httpContext.Request.Method == "GET")
            {
                httpContext.Response.StatusCode = 200;
                await httpContext.Response.WriteAsync(string.Empty);
                return;
            }
                    
            await _next(httpContext);
        }
    }

    public static class MethodFilteringExtensions
    {
        public static IApplicationBuilder UseMethodFiltering(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MethodFiltering>();
        }
    }
}
