using Kukirmash.API.Contracts.TextMessage;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Services;
using Kukirmash.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

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
                                                    ITextMessageService textMessageService,
                                                    IUserService userService )
    {
        try
        {
            // 1. Не забываем про защиту от Guid? для текущего пользователя!
            Guid userId = userClaims.GetUserId();

            List<TextMessage> messages = await textMessageService.Get( count, userId, serverId, channelId );

            // 2. Получаем всех уникальных авторов (тип здесь IEnumerable<Guid?>)
            var creatorIds = messages.Select( m => m.CreatorId ).Distinct();

            var usersLogin = new Dictionary<Guid?, string>();

            // ВАЖНО: В цикле используем Guid? 
            foreach ( Guid? cId in creatorIds )
            {
                if ( cId.HasValue ) // Если аккаунт не удален
                {
                    // Передаем cId.Value, чтобы получить чистый Guid для метода GetById
                    var user = await userService.GetById( cId.Value );
                    usersLogin[cId] = user != null ? user.Login : "Неизвестный";
                }
                else // Если CreatorId == null (аккаунт удален)
                {
                    usersLogin[cId] = "Удаленный пользователь";
                }
            }

            // 3. Собираем итоговый ответ
            var messagesResponse = messages.Select( x => new GetTextMessageResponse(
                x.Id,
                x.Text,
                x.CreatedDateTimeUtc,
                usersLogin[x.CreatorId]
            ) );

            return Results.Ok( messagesResponse );

        }
        catch (Exception ex)
        {
            return Results.Problem();
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------
}
