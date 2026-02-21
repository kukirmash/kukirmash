using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface ITextChannelRepository
{
    Task Add(TextChannel textChannel, Guid serverId);
    Task<List<TextMessage>> GetTextMessages(Guid textChannelId);
}
