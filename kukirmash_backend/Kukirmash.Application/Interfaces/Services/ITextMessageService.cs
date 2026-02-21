using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface ITextMessageService
{
    Task<TextMessage> Add(string text, Guid creatorId, Guid serverId, Guid textChannelId);
}
