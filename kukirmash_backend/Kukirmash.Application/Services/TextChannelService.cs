using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;

namespace Kukirmash.Application.Services;

public class TextChannelService : ITextChannelService
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly ITextChannelRepository _textChannelRepository;

    private readonly IServerRepository _serverRepository;

    public TextChannelService(ITextChannelRepository textChannelRepository, IServerRepository serverRepository)
    {
        _textChannelRepository = textChannelRepository;
        _serverRepository = serverRepository;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(Guid id, string name, Guid userId, Guid serverId)
    {
        var server = await _serverRepository.GetById(serverId);

        if (server is null)
            throw new KeyNotFoundException($"Сервер с ID {serverId} не найден");

        if (server.CreatorId != userId)
            throw new InvalidOperationException($"Пользователь с ID {userId} не является создателем сервера, и не может создавать тектсовые каналы");

        TextChannel textChannel = TextChannel.Create(id, name, serverId);

        await _textChannelRepository.Add(textChannel, serverId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public Task<List<TextMessage>> GetTextMessages(Guid textChannelId, Guid userId)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
