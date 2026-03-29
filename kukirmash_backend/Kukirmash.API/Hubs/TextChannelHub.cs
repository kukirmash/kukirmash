using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Services;
using Kukirmash.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Kukirmash.API.Hubs;

[Authorize] // SignalR сам достанет твой JWT из куки
public class TextChannelHub : Hub
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly ITextMessageService _messageService;
    private readonly IUserService _userService;

    public TextChannelHub( ITextMessageService messageService, IUserService userService )
    {
        _messageService = messageService;
        _userService = userService;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // 1. Подключение фронтенда к текстовому каналу
    public async Task JoinChannel( string textChannelId )
    {
        await Groups.AddToGroupAsync( Context.ConnectionId, textChannelId );
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // 2. Отключение от канала
    public async Task LeaveChannel( string textChannelId )
    {
        await Groups.RemoveFromGroupAsync( Context.ConnectionId, textChannelId );
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // 3. Отправка сообщения
    public async Task SendMessage( string serverIdStr, string textChannelIdStr, string text )
    {
        Guid userId = Context.User.GetUserId();
        Guid serverId = Guid.Parse( serverIdStr );
        Guid textChannelId = Guid.Parse( textChannelIdStr );

        // Сохраняем в базу данных
        TextMessage textMessage = await _messageService.Add( text, userId, serverId, textChannelId );

        var user = await _userService.GetById( userId ); // Убедись, что такой метод есть в сервисе

        string login = "Неизвестный";

        if ( user != null )
            login = user.Login;

        // 3. Формируем DTO объект для отправки на фронт, включая ИМЯ АВТОРА
        var messageDto = new
        {
            Id = textMessage.Id,
            Text = textMessage.Text,
            CreatedDateTimeUtc = textMessage.CreatedDateTimeUtc,
            AuthorName = login
        };

        // Отправляем сообщение обратно ВСЕМ, кто сейчас в этом канале
        await Clients.Group( textChannelIdStr ).SendAsync( "ReceiveMessage", messageDto );
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}