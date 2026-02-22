using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kukirmash.Persistence.Repositories;

public class TextChannelRepository : ITextChannelRepository
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly KukirmashDbContext _context;

    private const string TAG = "TextChannelRepository";

    public TextChannelRepository(KukirmashDbContext context)
    {
        _context = context;
    }
    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(TextChannel textChannel, Guid serverId)
    {
        var serverEntity = await _context.Servers
            .FindAsync(serverId);

        if (serverEntity == null)
            throw new KeyNotFoundException($"Сервер с ID {serverId} не найден");

        var textChannelEntity = new TextChannelEntity()
        {
            Id = textChannel.Id,
            Name = textChannel.Name,
            ServerId = serverId
        };

        Log.Information("{TAG} - добавление нового текстовго канала... (ID:{id})", TAG, textChannel.Id);

        await _context.TextChannels.AddAsync(textChannelEntity);
        await _context.SaveChangesAsync();

        Log.Information("{TAG} - текстовый канал добавлен успешно. (ID:{id})", TAG, textChannel.Id);
    }

    public Task<List<TextMessage>> GetTextMessages(Guid textChannelId)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
