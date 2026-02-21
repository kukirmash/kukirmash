using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface ITextChannelService
{
    Task Add(Guid id, string name, Guid userId, Guid serverId);

    Task<List<TextChannel>> GetAllTextChannels(Guid serverId, Guid userId);
}
