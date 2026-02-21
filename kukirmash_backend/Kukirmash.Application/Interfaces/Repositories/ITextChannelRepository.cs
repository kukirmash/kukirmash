using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface ITextChannelRepository
{
    Task Add(TextChannel textChannel, Guid serverId);

    Task<List<TextChannel>> GetAllTextChannels(Guid serverId);
}
