using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace LoginUsingMiddleware.Middleware
{
    public class InputValidation
    {
        private readonly RequestDelegate _next;

        public InputValidation(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Parse body.
            var reader = new StreamReader(httpContext.Request.Body);
            string body = await reader.ReadToEndAsync();
            Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(body);

            string? email = null;
            string? password = null;
            if (queryDict.ContainsKey("email")) email = Convert.ToString(queryDict["email"][0]);
            if (queryDict.ContainsKey("password")) password = Convert.ToString(queryDict["password"][0]);

            // Validate: 400 Bad request if both email & password are not provided.
            if (email is null || password is null)
            {
                httpContext.Response.StatusCode = 400;
                if (email is null) await httpContext.Response.WriteAsync("Invalid input for 'email'\n");
                if (password is null) await httpContext.Response.WriteAsync("Invalid input for 'password'\n");
                return;
            }

            // Authentication: 400 Bad request if both email & password are not exact matches.
            string EMAIL = "admin@example.com";
            string PASSWORD = "admin1234";

            if (email == EMAIL && password == PASSWORD)
            {
                httpContext.Response.StatusCode = 200;
                await httpContext.Response.WriteAsync("Successful login\n");
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Invalid login\n");
            }
        }
    }

    public static class InputValidationExtensions
    {
        public static IApplicationBuilder UseInputValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<InputValidation>();
        }
    }
}
