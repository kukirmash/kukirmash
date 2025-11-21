using System.Text.RegularExpressions;
using Kukirmash.API.Contracts.User;
using Kukirmash.Application.Exceptions;
using Kukirmash.Application.Interfaces.Services;

namespace Kukirmash.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", Register);
        app.MapPost("login", Login);

        return app;
    }

    private static async Task<IResult> Register(RegisterUserRequest registerUserRequest, IUserService userService)
    {
        if (string.IsNullOrWhiteSpace(registerUserRequest.Login))
            return Results.BadRequest("Login is required");

        if (string.IsNullOrWhiteSpace(registerUserRequest.Email))
            return Results.BadRequest("Email is required");

        string pattern = @"^(?!\.)(""([^""\r\\]|\\.)+""|([-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+(?:\.[-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+)*))"
                + @"@([a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[A-Za-z]{2,}$";

        if (!Regex.IsMatch(registerUserRequest.Email, pattern, RegexOptions.IgnoreCase))
            return Results.BadRequest("Invalid email format");

        if (string.IsNullOrWhiteSpace(registerUserRequest.Password) || registerUserRequest.Password.Length < 8)
            return Results.BadRequest("Password must be at least 8 characters");

        try
        {
            await userService.Register(
                registerUserRequest.Login,
                registerUserRequest.Email,
                registerUserRequest.Password
            );
        }
        catch (UserAlreadyExistsException)
        {
            return Results.Conflict("User with this login or email already exists");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }

        return Results.Ok("User successfully registered");

    }

    private static async Task<IResult> Login(LoginUserRequest loginUserRequest, IUserService userService)
    {
        var token = "";

        if (loginUserRequest.Login.Contains("@"))
        {
            token = await userService.LoginByEmail(
            loginUserRequest.Login,
            loginUserRequest.Password
            );
        }
        else
        {
            token = await userService.LoginByLogin(
            loginUserRequest.Login,
            loginUserRequest.Password
            );
        }


        return Results.Ok(token);
    }
}
