using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entites;
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

    //*----------------------------------------------------------------------------------------------------------------------------
}
