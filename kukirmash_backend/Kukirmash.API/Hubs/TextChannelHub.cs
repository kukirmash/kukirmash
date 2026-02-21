using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Kukirmash.API.Extensions;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;

namespace Kukirmash.API.Hubs;

[Authorize] // SignalR сам достанет твой JWT из куки
public class TextChannelHub : Hub
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly ITextMessageService _messageService;

    public TextChannelHub(ITextMessageService messageService)
    {
        _messageService = messageService;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // 1. Подключение фронтенда к текстовому каналу
    public async Task JoinChannel(string textChannelId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, textChannelId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // 2. Отключение от канала
    public async Task LeaveChannel(string textChannelId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, textChannelId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // 3. Отправка сообщения
    public async Task SendMessage(string serverIdStr, string textChannelIdStr, string text)
    {
        Guid userId = Context.User.GetUserId();
        Guid serverId = Guid.Parse(serverIdStr);
        Guid textChannelId = Guid.Parse(textChannelIdStr);

        // Сохраняем в базу данных
        TextMessage textMessage = await _messageService.Add(text, userId, serverId, textChannelId);

        // Отправляем сообщение обратно ВСЕМ, кто сейчас в этом канале
        await Clients.Group(textChannelIdStr).SendAsync("ReceiveMessage", textMessage);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}