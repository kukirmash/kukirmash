using System.Security.Claims;
using Kukirmash.API.Contracts.TextMessage;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kukirmash.API.Endpoints;

public static class TextMessageEndpoints
{
    //----------------------------------------------------------------------------------------------------------------------------
    public static IEndpointRouteBuilder MapTextMessageEndpoints(this IEndpointRouteBuilder app)
    {
        var textMessageGroup = app.MapGroup("servers/{serverId:guid}/text-channels/{channelId:guid}")
            .RequireAuthorization();


        textMessageGroup.MapGet("/messages/{count:int}", GetMessages);

        return app;
    }

    //----------------------------------------------------------------------------------------------------------------------------
    private static async Task<IResult> GetMessages([FromRoute] Guid serverId,
                                                    [FromRoute] Guid channelId,
                                                    [FromRoute] int count,
                                                    ClaimsPrincipal userClaims,
                                                    ITextMessageService textMessageService)
    {
        try
        {
            Guid userId = userClaims.GetUserId();

            List<TextMessage> messages = await textMessageService.Get(count, userId, serverId, channelId);

            var messagesResponse = messages.Select(x => new GetTextMessageResponse(x.Id, x.Text, x.CreatedDateTimeUtc));

            return Results.Ok(messagesResponse);

        }
        catch (Exception ex)
        {
            return Results.Problem();
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------
}
