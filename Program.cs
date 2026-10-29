using Microsoft.AspNetCore.Identity;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Repositories;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection is missing from configuration.");

builder.Services.AddSingleton(
    NpgsqlDataSource.Create(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddValidation();


builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordHasher<User>>();
var app = builder.Build();

app.MapGet("/", () =>
{
    return  "Hello World!";
});

app.MapGet("/api/users", async (UserService userService) =>
{
    var users = await userService.GetAllUsersAsync();
    return Results.Ok(users.Select(u => new
    {
        u.id,
        u.username,
        u.email
    }));
});

app.MapPost("/api/users/register", async (RegisterUserDto request, UserService userService) =>
{
    var user = await userService.RegisterAsync(request);

    if (user is null)
    {
        return Results.BadRequest(new
        {
            message = "Invalid registration data or username/email already taken."
        });
    }
    
    return Results.Created(
        $"/api/users/{user.id}",
        new
        {
            user.id,
            user.username,
            user.email
        }
    );
});

app.MapPost("/api/users/login", async (LoginUserDto request, UserService userService) =>
{
    var user = await userService.LoginAsync(request);

    if (user is null)
    {
        return Results.BadRequest(new
        {
            message = "Invalid login data or username/email already taken."
        });
    }

    return Results.Ok(new
    {
        message = "Successfully logged in.",
        user.id,
        user.username,
        user.email
    });

});


app.Run();