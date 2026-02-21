using System;
using Kukirmash.Core.Models;

namespace Kukirmash.Persistence.Entites;

public class TextMessageEntity
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedDateTimeUtc { get; set; }
    public Guid TextChannelId { get; set; }
    public Guid? CreatorId { get; set; } // пользователь может удалить акк



    public TextChannelEntity TextChannel { get; set; } = null!;
    public UserEntity Creator { get; set; } = null!;
}
