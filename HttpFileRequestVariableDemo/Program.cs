var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string HardcodedToken = "1234567890";

// Endpoint to generate a token
app.MapPost("/users/token", (HttpContext httpContext) =>
{
    var username = httpContext.Request.Form["username"];
    var password = httpContext.Request.Form["password"];

    if (username == "admin" && password == "password")
    {
        return Results.Ok(new { token = HardcodedToken });
    }

    return Results.Unauthorized();
});

// Endpoint to validate the token
app.MapGet("/todos", (HttpContext httpContext) =>
{
    // Retrieve the token from the Authorization header
    var authorizationHeader = 
        httpContext.Request.Headers["Authorization"].FirstOrDefault();

    if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
    {
        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        if (token == HardcodedToken)
        {
            return Results.Ok(new
            {
                todos = new[]
                {
                    new { id = 1, task = "Learn .NET", completed = false },
                    new { id = 2, task = "Build cool projects", completed = true },
                }
            });
        }
    }

    return Results.Unauthorized();
});

app.Run();