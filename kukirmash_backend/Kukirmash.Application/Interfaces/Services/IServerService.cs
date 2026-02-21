using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface IServerService
{
    // Сервер
    Task Add(Guid creatorId, string name, string? desc, bool isPrivate);
    Task Add(Guid creatorId, string name, string? desc, Stream iconStream, string fileName, bool isPrivate);
    Task<List<Server>> GetAllServers(bool? isPrivate = null);

    // Участники сервера
    Task AddUser(Guid serverId, Guid userId);
    Task<List<User>> GetMembers(Guid serverId);
    Task<string> GetInviteToken(Guid serverId); // TODO:

    // Текстовые каналы
    Task<List<TextChannel>> GetTextChannels(Guid serverId, Guid userId);
}
