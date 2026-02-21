using Kukirmash.Core.Models;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces;

namespace Kukirmash.Application.Services;

public class ServerService : IServerService
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly IServerRepository _serverRepository;

    private readonly IStaticFileService _staticFileService;

    public ServerService(IServerRepository serverRepository, IStaticFileService staticFileService, IUserRepository userRepository)
    {
        _serverRepository = serverRepository;
        _staticFileService = staticFileService;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // Добавляет сервер без фото
    public async Task Add(Guid creatorId, string name, string? desc, bool isPrivate)
    {
        // создаем модель сервера без фото
        var server = Server.Create(Guid.NewGuid(), name, desc, null, isPrivate, creatorId);

        // добавляем в БД
        await _serverRepository.Add(server);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // Добавляет сервер с фото
    public async Task Add(Guid creatorId, string name, string? desc, Stream iconStream, string fileName, bool isPrivate)
    {
        // Сохраняем фото -> отностельный путь где он сохранен, в папке wwwroot
        string iconPath = await _staticFileService.UploadFile(iconStream, fileName, "media/server-icons");

        // создаем модель сервера
        var server = Server.Create(Guid.NewGuid(), name, desc, iconPath, isPrivate, creatorId);

        // добавляем в БД
        await _serverRepository.Add(server);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetAllServers(bool? isPrivate = null)
    {
        List<Server> servers = await _serverRepository.GetAllServers(isPrivate);

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public Task<string> GetInviteToken(Guid serverId)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<User>> GetServerUsers(Guid serverId)
    {
        List<User> serverMembers = await _serverRepository.GetMembers(serverId);

        return serverMembers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task AddUser(Guid serverId, Guid userId)
    {
        await _serverRepository.AddUser(serverId, userId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
