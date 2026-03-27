using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;

namespace Kukirmash.Application.Services;

public class TextMessageService : ITextMessageService
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly ITextMessageRepository _textMessageRepository;
    private readonly IServerRepository _serverRepository;

    public TextMessageService(ITextMessageRepository textMessageRepository, IServerRepository serverRepository)
    {
        _textMessageRepository = textMessageRepository;
        _serverRepository = serverRepository;
    }

    //----------------------------------------------------------------------------------------------------------------------------
    public async Task<TextMessage> Add(string text, Guid creatorId, Guid serverId, Guid textChannelId)
    {
        var server = await _serverRepository.GetById(serverId);
        if (server is null)
            throw new KeyNotFoundException($"Сервер с ID {serverId} не найден");

        bool isMember = await _serverRepository.IsMember(serverId, creatorId);
        if (!isMember)
            throw new InvalidOperationException($"Пользователь c ID {creatorId} не является участником сервера");

        bool hasTextChannel = await _serverRepository.HasTextChannel(serverId, textChannelId);
        if (!hasTextChannel)
            throw new InvalidOperationException($"На сервере с ID {serverId} нет текстового канала с ID {textChannelId} ");

        TextMessage message = TextMessage.CreateNew(Guid.NewGuid(), text, creatorId, textChannelId);

        var savedMessage = await _textMessageRepository.Add(message);

        return savedMessage;
    }

    //----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<TextMessage>> Get(int count, Guid userId, Guid serverId, Guid textChannelId)
    {
        var server = await _serverRepository.GetById(serverId);
        if (server is null)
            throw new KeyNotFoundException($"Сервер с ID {serverId} не найден");

        bool isMember = await _serverRepository.IsMember(serverId, userId);
        if (!isMember)
            throw new InvalidOperationException($"Пользователь c ID {userId} не является участником сервера");

        bool hasTextChannel = await _serverRepository.HasTextChannel(serverId, textChannelId);
        if (!hasTextChannel)
            throw new InvalidOperationException($"На сервере с ID {serverId} нет текстового канала с ID {textChannelId} ");

        List<TextMessage> messages = await _textMessageRepository.Get(count, serverId, textChannelId);

        return messages;
    }

    //----------------------------------------------------------------------------------------------------------------------------
}
