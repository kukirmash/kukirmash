using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kukirmash.Persistence.Repositories;

public class TextMessageRepository : ITextMessageRepository
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly KukirmashDbContext _context;
    private const string TAG = "TextMessageRepository";

    public TextMessageRepository(KukirmashDbContext context)
    {
        _context = context;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<TextMessage> Add(TextMessage message)
    {
        var textMessageEntity = new TextMessageEntity
        {
            Id = message.Id,
            Text = message.Text,
            CreatedDateTimeUtc = message.CreatedDateTimeUtc,
            CreatorId = message.CreatorId,
            TextChannelId = message.TextChannelId
        };

        Log.Information("{TAG} - добавление нового текстовго сообщений... (ID:{Id})", TAG, message.Id);

        // Добавление 
        await _context.TextMessages.AddAsync(textMessageEntity);
        await _context.SaveChangesAsync();

        Log.Information("{TAG} - текстовое сообщение добавлено успешно. (ID:{Id})", TAG, message.Id);

        return message;
    }

    //----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<TextMessage>> Get(int count, Guid serverId, Guid textChannelId)
    {
        Log.Information("{TAG} - получение последних {Count} сообщений для канала {ChannelId}...", TAG, count, textChannelId);

        // 1. Получаем сущности из базы данных
        var messageEntities = await _context.TextMessages
            .AsNoTracking() // Ускоряет запрос, так как нам не нужно отслеживать изменения этих объектов
            .Where(m => m.TextChannelId == textChannelId) // Фильтруем по ID канала
            .OrderByDescending(m => m.CreatedDateTimeUtc) // Сортируем по убыванию времени (сначала новые)
            .Take(count) // Берем нужное количество
            .ToListAsync();

        // 2. Маппим сущности БД (TextMessageEntity) обратно в доменные модели (TextMessage)
        var messages = messageEntities.Select(e => TextMessage.Create
        (
            e.Id,
            e.Text,
            e.CreatedDateTimeUtc,
            e.CreatorId,
            e.TextChannelId
        )).ToList();

        // 3. Переворачиваем список, чтобы сообщения шли в хронологическом порядке (старые выше, новые ниже)
        messages.Reverse();

        return messages;
    }

    //----------------------------------------------------------------------------------------------------------------------------
}
