using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Conditional middleware for HTTP GET request: 200 Empty Response.
app.UseWhen(
    context => context.Request.Method == "GET",
    app => {
        app.Run(async (HttpContext context) =>
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(string.Empty);
        });
    }
);

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    var reader = new StreamReader(context.Request.Body);
    string body = await reader.ReadToEndAsync();

    Dictionary<string, StringValues> queryDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

    // Validate: 400 Bad request if both email & password are not provided.
    if (!queryDict.ContainsKey("email") ||
        !queryDict.ContainsKey("password"))
    {
        context.Response.StatusCode = 400;

        if (!queryDict.ContainsKey("email"))
        {
            await context.Response.WriteAsync("Invalid input for 'email'\n");
        }

        if (!queryDict.ContainsKey("password"))
        {
            await context.Response.WriteAsync("Invalid input for 'password'\n");
        }
        
        return;
    }

    // Validate: 400 Bad request if both email & password are not exact matches.
    if (queryDict["email"] == "admin@example.com" &&
        queryDict["password"] == "admin1234")
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("Successful login\n");
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Invalid login\n");
    }
});

app.Run();
