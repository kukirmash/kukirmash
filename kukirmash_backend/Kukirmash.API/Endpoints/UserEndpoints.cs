using System.Security.Authentication;
using System.Security.Claims;
using System.Text.RegularExpressions;
using FluentValidation;
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
        // Группа "/users"
        var usersGroup = app.MapGroup("users");

        // GET /users
        usersGroup.MapGet("/", GetAllUsers)
            .RequireAuthorization();

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
    private static async Task<IResult> Register(
        RegisterUserRequest request,
        IUserService userService,
        IValidator<RegisterUserRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid == false)
            return Results.ValidationProblem(validationResult.ToDictionary());

        try
        {
            // Регистрируем нового пользователя
            await userService.Register(request.Login, request.Email, request.Password);
            return Results.Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message); // Возвращаем 409 Conflict
        }
        catch (Exception ex) // Любая другая ошибка
        {
            Log.Error(ex, $"Ошибка регистрации {ex.Message}");
            return Results.Problem("Произошла неизвестная ошибка");
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> Login(
        LoginUserRequest request,
        IUserService userService,
        HttpContext httpContext,
        IValidator<LoginUserRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid == false)
            return Results.ValidationProblem(validationResult.ToDictionary());

        try
        {
            // Находим пользователя -> генерируем для него jwt токен (время жизни токена 12 часов)
            string token = string.Empty;

            if (request.Login.Contains("@"))
                token = await userService.LoginByEmail(request.Login, request.Password);
            else
                token = await userService.LoginByLogin(request.Login, request.Password);

            // Заносим сгенерированный токен в cookies
            httpContext.Response.Cookies.Append("big-balls", token);

            return Results.Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message); // 404
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message); // 400 
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Ошибка логина {ex.Message}");
            return Results.Problem("Произошла неизвестная ошибка");
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
            Log.Error(ex, $"Ошибка получения всех пользвателей {ex.Message}");
            return Results.Problem("Произошла неизвестная ошибка");
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
        catch (AuthenticationException)
        {
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Ошибка получения серверов у пользователя: {ex.Message}");
            return Results.Problem("Произошла неизвестная ошибка");
        }

    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
