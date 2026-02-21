using System;

namespace Kukirmash.Persistence.Entites;

public class TextChannelEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ServerId { get; set; }


    public ServerEntity Server { get; set; } = null!;
    public List<TextMessageEntity> TextMessages { get; set; } = [];
}