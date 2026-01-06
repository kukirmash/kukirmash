using System.Security.Authentication;
using System.Security.Claims;
using Kukirmash.API.Contracts.Server;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Kukirmash.API.Endpoints;

public static class ServerEndpoints
{
    //*----------------------------------------------------------------------------------------------------------------------------
    public static IEndpointRouteBuilder MapServerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("server", GetAllServers);
        app.MapPost("server", AddServer)
            .RequireAuthorization()
            .DisableAntiforgery();

        return app;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> AddServer(
        [FromForm] AddServerRequest addServerRequest,
        IServerService serverService,
        ClaimsPrincipal userClaims)
    {
        if (string.IsNullOrWhiteSpace(addServerRequest.Name))
            return Results.BadRequest("Server name is required");

        try
        {
            Guid userId = userClaims.GetUserId();

            // Превращаем IFormFile в поток, чтобы Application слой не знал про HTTP
            Stream iconStream = addServerRequest.Icon.OpenReadStream();
            string fileName = addServerRequest.Icon.FileName;

            await serverService.Add(userId, addServerRequest.Name, addServerRequest.Description, iconStream, fileName);

            return Results.Ok();
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

    //*----------------------------------------------------------------------------------------------------------------------------
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

    //*----------------------------------------------------------------------------------------------------------------------------
}
