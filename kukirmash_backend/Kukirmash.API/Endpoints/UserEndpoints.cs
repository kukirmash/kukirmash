using Kukirmash.API.Contracts.User;
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
        await userService.Register(
            registerUserRequest.Login,
            registerUserRequest.Email,
            registerUserRequest.Password
        );

        return Results.Ok();
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
