using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface IServerRepository
{
    Task Add(Server server, Guid creatorId);

    Task AddUser(Guid serverId, User user);

    Task<Server> GetById(Guid Id);

    Task<List<Server>> GetAllServers();
    Task<List<Server>> GetPublicServers();
    Task<List<Server>> GetPrivateServers();

    Task<User> GetCreator(Guid serverId);

    Task<List<User>> GetMembers(Guid serverId);

    Task<bool> IsMember(Guid serverId, User user);
}
