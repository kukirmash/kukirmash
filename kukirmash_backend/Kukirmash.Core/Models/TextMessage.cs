namespace Kukirmash.Core.Models;

public class TextMessage
{
    public Guid Id { get; private set; }
    public string Text { get; private set; }
    public DateTime CreatedDateTimeUtc { get; private set; }


    public Guid TextChannelId { get; private set; }
    public Guid? CreatorId { get; private set; } // пользователь может удалить акк

    private TextMessage(Guid id, string text, DateTime createdDateTimeUtc, Guid? creatorId, Guid textChannelId)
    {
        Id = id;
        Text = text;
        CreatedDateTimeUtc = createdDateTimeUtc;
        CreatorId = creatorId;
        TextChannelId = textChannelId;
    }

    public static TextMessage CreateNew(Guid id, string text, Guid? creatorId, Guid textChannelId)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Текст сообщения не может быть пустым", nameof(text));

        if (creatorId == Guid.Empty)
            throw new ArgumentException("Сообщение должно иметь отправителя", nameof(creatorId));

        if (textChannelId == Guid.Empty)
            throw new ArgumentException("Сообщение должно принадлежать текстовому каналу", nameof(textChannelId));

        DateTime createdAt = DateTime.UtcNow;
        return new TextMessage(id, text, createdAt, creatorId, textChannelId);
    }

    public static TextMessage Create(Guid id, string text, DateTime createdDateTimeUtc, Guid? creatorId, Guid textChannelId)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Текст сообщения не может быть пустым", nameof(text));

        if (creatorId == Guid.Empty)
            throw new ArgumentException("Сообщение должно иметь отправителя", nameof(creatorId));

        if (textChannelId == Guid.Empty)
            throw new ArgumentException("Сообщение должно принадлежать текстовому каналу", nameof(textChannelId));

        return new TextMessage(id, text, createdDateTimeUtc, creatorId, textChannelId);
    }
}
