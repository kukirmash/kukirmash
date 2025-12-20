using System.Text.RegularExpressions;
using Kukirmash.API.Contracts;
using Kukirmash.API.Contracts.User;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;
using Serilog;

namespace Kukirmash.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        // TODO: разграничить endpointы для залогининых
        app.MapPost("register", Register);
        app.MapPost("login", Login);
        app.MapGet("users", Users);

        return app;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> Register(RegisterUserRequest registerUserRequest, IUserService userService)
    {
        // Проверяем логин
        // TODO: добавить запрещенные символы в логин (например @)
        if (string.IsNullOrWhiteSpace(registerUserRequest.Login))
        {
            Log.Information("Bad request: Login is required");
            return Results.BadRequest("Login is required");
        }


        // Проверяем почту
        if (string.IsNullOrWhiteSpace(registerUserRequest.Email))
        {
            Log.Information("Bad request: Email is required");
            return Results.BadRequest("Email is required");
        }

        string pattern = @"^(?!\.)(""([^""\r\\]|\\.)+""|([-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+(?:\.[-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+)*))"
                + @"@([a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[A-Za-z]{2,}$";

        if (!Regex.IsMatch(registerUserRequest.Email, pattern, RegexOptions.IgnoreCase))
        {
            Log.Information("Bad request: Invalid email format");
            return Results.BadRequest("Invalid email format");
        }

        // Проверяем пароль: длина >= 8 символов
        if (string.IsNullOrWhiteSpace(registerUserRequest.Password) || registerUserRequest.Password.Length < 8)
        {
            Log.Information("Bad request: Password must be at least 8 characters");
            return Results.BadRequest("Password must be at least 8 characters");
        }

        // Регистрируем нового пользователя
        try
        {
            await userService.Register(registerUserRequest.Login, registerUserRequest.Email, registerUserRequest.Password);
        }
        catch (Exception ex)
        {
            Log.Information($"Problem: {ex.Message}");
            return Results.Problem(ex.Message);
        }

        return Results.Ok("User successfully registered");

    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> Login(LoginUserRequest loginUserRequest, IUserService userService, HttpContext httpContext)
    {
        // Проверяем логин
        if (string.IsNullOrWhiteSpace(loginUserRequest.Login))
            return Results.BadRequest("Login is required");

        // Проверяем пароль: длина >= 8 символов
        if (string.IsNullOrWhiteSpace(loginUserRequest.Password) || loginUserRequest.Password.Length < 8)
            return Results.BadRequest("Password must be at least 8 characters");

        // Находим пользователя -> генерируем для него jwt токен (время жизни токена 12 часов)
        var token = "";

        try
        {
            if (loginUserRequest.Login.Contains("@"))
                token = await userService.LoginByEmail(loginUserRequest.Login, loginUserRequest.Password);
            else
                token = await userService.LoginByLogin(loginUserRequest.Login, loginUserRequest.Password);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }

        // Заносим сгенерированный токен в cookies
        httpContext.Response.Cookies.Append("big-balls", token);

        return Results.Ok(token);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> Users(IUserService userService)
    {
        try
        {
            List<User> users = await userService.GetAllUsers();

            // Убираем пароль из списка
            var response = users.Select(u => new UsersResponse( u.Id, u.Login,u.Email));

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
