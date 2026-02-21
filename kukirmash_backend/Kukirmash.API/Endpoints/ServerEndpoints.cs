using System.Security.Authentication;
using System.Security.Claims;
using Kukirmash.API.Contracts.User;
using Kukirmash.API.Contracts.Server;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using FluentValidation;

namespace Kukirmash.API.Endpoints;

public static class ServerEndpoints
{
    //*----------------------------------------------------------------------------------------------------------------------------
    public static IEndpointRouteBuilder MapServerEndpoints(this IEndpointRouteBuilder app)
    {
        var serverGroup = app.MapGroup("servers")
        .RequireAuthorization();

        serverGroup.MapGet("/", GetAllServers);
        serverGroup.MapGet("public", GetPublicServers);
        serverGroup.MapGet("private", GetPrivateServers);
        serverGroup.MapGet("{id:guid}/users", GetServerUsers);
        serverGroup.MapPost("{id:guid}/join", JoinToServer);
        serverGroup.MapPost("/", AddServer)
            .DisableAntiforgery();

        return app;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> AddServer(
        [FromForm] AddServerRequest request,
        IServerService serverService,
        ClaimsPrincipal userClaims,
        IValidator<AddServerRequest> validator)
    {

        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid == false)
            return Results.ValidationProblem(validationResult.ToDictionary());

        try
        {
            Guid userId = userClaims.GetUserId();

            // Добавляем сервер без фото
            if (request.Icon is null)
            {
                await serverService.Add(userId, request.Name, request.Description, request.IsPrivate);
                return Results.Ok();
            }

            // Превращаем IFormFile в поток, чтобы Application слой не знал про HTTP
            using Stream iconStream = request.Icon.OpenReadStream();
            string fileName = request.Icon.FileName; // нужен для расшерния фото

            // Добавляем сервер вместе с фото
            await serverService.Add(
                userId,
                request.Name,
                request.Description,
                iconStream,
                fileName,
                request.IsPrivate);

            return Results.Ok();
        }
        catch (AuthenticationException)
        {
            return Results.Unauthorized();
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message); // 404
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
            List<Server> servers = await serverService.GetAllServers(null);

            var serversResponse = servers.Select(s => new ServerResponse(
                s.Id,
                s.Name,
                s.Description,
                s.IconPath,
                s.IsPrivate));

            return Results.Ok(serversResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> GetPublicServers(IServerService serverService)
    {
        try
        {
            List<Server> servers = await serverService.GetAllServers(false);

            var serversResponse = servers.Select(s => new ServerResponse(
                s.Id,
                s.Name,
                s.Description,
                s.IconPath,
                s.IsPrivate));

            return Results.Ok(serversResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> GetPrivateServers(IServerService serverService)
    {
        try
        {
            List<Server> servers = await serverService.GetAllServers(true);

            var serversResponse = servers.Select(s => new ServerResponse(
                s.Id,
                s.Name,
                s.Description,
                s.IconPath,
                s.IsPrivate));

            return Results.Ok(serversResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> GetServerUsers([FromRoute] Guid id, IServerService serverService)
    {
        try
        {
            var users = await serverService.GetMembers(id);

            return Results.Ok(users.Select(u => new UsersResponse(u.Id, u.Login, u.Email)));
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message); // 404
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }

    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> JoinToServer([FromRoute] Guid id, IServerService serverService, ClaimsPrincipal userClaims)
    {
        try
        {
            Guid userId = userClaims.GetUserId();

            await serverService.AddUser(id, userId);

            return Results.Ok();
        }
        catch (AuthenticationException)
        {
            return Results.Unauthorized();
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
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
