using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface IServerRepository
{
    // Сервер
    Task Add(Server server);
    Task<Server?> GetById(Guid Id);
    Task<List<Server>> GetAllServers(bool? isPrivate = null);

    // Участники сервера
    Task AddUser(Guid serverId, Guid userId);
    Task<User?> GetCreator(Guid serverId);
    Task<List<User>> GetMembers(Guid serverId);
    Task<bool> IsMember(Guid serverId, Guid userId);

    // Текстовые каналы
    Task<List<TextChannel>> GetTextChannels(Guid serverId);
    Task<bool> HasTextChannel(Guid serverId, Guid textChannelId);
}
