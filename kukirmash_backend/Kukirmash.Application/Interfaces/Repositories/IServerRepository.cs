using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface IServerRepository
{
    Task Add(Server server);
    Task AddUser(Guid serverId, Guid userId);
    Task<bool> IsMember(Guid serverId, Guid userId);

    Task<List<Server>> GetAllServers(bool? isPrivate = null);
    Task<Server?> GetById(Guid Id);
    Task<User?> GetCreator(Guid serverId);
    Task<List<User>> GetMembers(Guid serverId);
}
