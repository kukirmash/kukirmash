using System.Security.Claims;
using System.Text.RegularExpressions;
using Kukirmash.API.Contracts;
using Kukirmash.API.Contracts.Server;
using Kukirmash.API.Contracts.User;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;
using Serilog;

namespace Kukirmash.API.Endpoints;

public static class UserEndpoints
{
    //*----------------------------------------------------------------------------------------------------------------------------
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        // Создаем группу "/users"
        var usersGroup = app.MapGroup("users");

        // GET /users
        usersGroup.MapGet("/", GetAllUsers);

        // GET /users/me/servers (Требует авторизацию)
        usersGroup.MapGet("me/servers", GetCurrentUserServers)
            .RequireAuthorization();

        // GET /users/{id}/servers (Просмотр серверов конкретного юзера)
        // {id:guid} - это констрейнт, маршрут сработает только если id это GUID
        //usersGroup.MapGet("{id:guid}/servers", GetUserServersById);

        app.MapPost("register", Register);
        app.MapPost("login", Login);

        return app;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> Register(RegisterUserRequest registerUserRequest, IUserService userService)
    {
        // Проверяем логин
        // TODO: добавить запрещенные символы в логин (например @)
        if (string.IsNullOrWhiteSpace(registerUserRequest.Login))
        {
            Log.Information("400 Bad request: Поле логин - обязательное");
            return Results.BadRequest("Поле логин - обязательное");
        }

        // Проверяем почту
        if (string.IsNullOrWhiteSpace(registerUserRequest.Email))
        {
            Log.Information("400 Bad request: Поле email - обязательное");
            return Results.BadRequest("Поле email - обязательное");
        }

        string pattern = @"^(?!\.)(""([^""\r\\]|\\.)+""|([-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+(?:\.[-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+)*))"
                + @"@([a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[A-Za-z]{2,}$";

        if (!Regex.IsMatch(registerUserRequest.Email, pattern, RegexOptions.IgnoreCase))
        {
            Log.Information("400 Bad request: Неверный формат email");
            return Results.BadRequest("Неверный формат email");
        }

        // Проверяем пароль: длина >= 8 символов
        if (string.IsNullOrWhiteSpace(registerUserRequest.Password) || registerUserRequest.Password.Length < 8)
        {
            Log.Information("400 Bad request: Минимальная длина пароля 8 символов");
            return Results.BadRequest("Минимальная длина пароля 8 символов");
        }

        try
        {
            // Регистрируем нового пользователя
            await userService.Register(registerUserRequest.Login, registerUserRequest.Email, registerUserRequest.Password);
            return Results.Ok("Пользователь успешно зарегестрирован");
        }
        catch (Exception ex)
        {
            Log.Information($"Problem: {ex.Message}");
            return Results.Problem(ex.Message);
        }
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

        try
        {
            // Находим пользователя -> генерируем для него jwt токен (время жизни токена 12 часов)
            var token = "";

            if (loginUserRequest.Login.Contains("@"))
                token = await userService.LoginByEmail(loginUserRequest.Login, loginUserRequest.Password);
            else
                token = await userService.LoginByLogin(loginUserRequest.Login, loginUserRequest.Password);

            // Заносим сгенерированный токен в cookies
            httpContext.Response.Cookies.Append("big-balls", token);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> GetAllUsers(IUserService userService)
    {
        try
        {
            List<User> users = await userService.GetAllUsers();

            // Убираем пароль и id из списка
            var usersResponse = users.Select(u => new UsersResponse(u.Id, u.Login, u.Email));

            return Results.Ok(usersResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> GetCurrentUserServers(IUserService userService, ClaimsPrincipal userClaims)
    {
        try
        {
            Guid userId = userClaims.GetUserId();

            List<Server> servers = await userService.GetUserServers(userId);

            var response = servers.Select(s => new ServerResponse(s.Id, s.Name, s.Description, s.IconPath, s.IsPrivate));

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }

    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
