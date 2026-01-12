using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface IServerService
{
    Task Add(Guid creatorId, string name, string? desc, bool isPrivate);
    Task Add(Guid creatorId, string name, string? desc, Stream iconStream, string fileName, bool isPrivate);

    Task<List<Server>> GetAllServers();
    Task<List<Server>> GetPublicServers();
    Task<List<Server>> GetPrivateServers();

    Task<string> GetInviteToken(Guid serverId);

    Task<List<User>> GetServerUsers(Guid serverId);

    Task AddUser(Guid serverId, Guid userId);
}
