using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface ITextMessageRepository
{
    Task<TextMessage> Add(TextMessage message);

    Task<List<TextMessage>> Get(int count, Guid serverId, Guid textChannelId);
}
