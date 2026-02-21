using System.Security.Authentication;
using System.Security.Claims;
using Kukirmash.API.Contracts.TextChannel;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Services;
using Kukirmash.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Kukirmash.API.Endpoints;

public static class TextChannelEndpoints
{
    //*----------------------------------------------------------------------------------------------------------------------------
    public static IEndpointRouteBuilder MapTextChannelEndpoints(this IEndpointRouteBuilder app)
    {
        // Создаем группу. Обрати внимание на название переменной пути: serverId
        var textChannelGroup = app.MapGroup("servers/{id:guid}/text-channels")
            .RequireAuthorization(); // Сразу защищаем все роуты в этой группе

        // GET /servers/{serverId}/text-channels (Получить все каналы сервера)
        textChannelGroup.MapGet("/", GetTextChannels);

        // POST /servers/{serverId}/text-channels (Создать канал в сервере)
        textChannelGroup.MapPost("/", AddTextChannel);

        return app;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> GetTextChannels([FromRoute] Guid id, ITextChannelService textChannelService, ClaimsPrincipal userClaims)
    {
        try
        {
            Guid userId = userClaims.GetUserId();

            List<TextChannel> textChannels = await textChannelService.GetAllTextChannels(id, userId);

            var textChannelResponse = textChannels.Select(x => new TextChannelResponse(x.Id, x.Name));

            return Results.Ok(textChannelResponse);
        }

        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> AddTextChannel(
        [FromRoute] Guid id,
        AddTextChannelRequest request,
        ITextChannelService textChannelService, ClaimsPrincipal userClaims)
    {
        try
        {
            Guid userId = userClaims.GetUserId();

            await textChannelService.Add(Guid.NewGuid(), request.Name, userId, id);

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
            return Results.BadRequest(ex.Message); // 400
        }
        catch (Exception ex)
        {
            Log.Information($"Problem: {ex.Message}");
            return Results.Problem(ex.Message);
        }
    }
}