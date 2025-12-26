using System.Security.Authentication;
using System.Security.Claims;
using Kukirmash.API.Contracts.Server;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;
using Serilog;

namespace Kukirmash.API.Endpoints;

public static class ServerEndpoints
{
    public static IEndpointRouteBuilder MapServerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("server", GetAllServers);
        app.MapPost("server", AddServer).RequireAuthorization();

        return app;
    }

    private static async Task<IResult> AddServer(AddServerRequest addServerRequest, IServerService serverService, ClaimsPrincipal user)
    {
        // TODO: проверки
        try
        {
            Guid userId = user.GetUserId();

            await serverService.Add(userId, addServerRequest.Name, addServerRequest.Description);

            return Results.Ok("Server successfully added");
        }
        catch (AuthenticationException)
        {
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            Log.Information($"Problem: {ex.Message}");
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetAllServers(IServerService serverService)
    {
        try
        {
            List<Server> servers = await serverService.GetAllServers();

            // Убираем из списка id
            var serversResponse = servers.Select(s => new ServerResponse(s.Name, s.Description));

            return Results.Ok(serversResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
