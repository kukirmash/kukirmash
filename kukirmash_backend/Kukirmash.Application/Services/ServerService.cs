using Kukirmash.Core.Models;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces;

namespace Kukirmash.Application.Services;

public class ServerService : IServerService
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly IServerRepository _serverRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStaticFileService _staticFileService;

    public ServerService(IServerRepository serverRepository, IStaticFileService staticFileService, IUserRepository userRepository)
    {
        _serverRepository = serverRepository;
        _staticFileService = staticFileService;
        _userRepository = userRepository;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // Добавляет сервер без фото
    public async Task Add(Guid creatorId, string name, string? desc, bool isPrivate)
    {
        // создаем модель сервера без фото
        var server = Server.Create(Guid.NewGuid(), name, desc, null, isPrivate);

        // добавляем в БД
        await _serverRepository.Add(server, creatorId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // Добавляет сервер с фото
    public async Task Add(Guid creatorId, string name, string? desc, Stream iconStream, string fileName, bool isPrivate)
    {
        // Пока нет проверок.
        // 1) Пользователь может создавать ограниченного колво серверов

        // Сохраняем фото -> отностельный путь где он сохранен, в папке wwwroot
        string iconPath = await _staticFileService.UploadFile(iconStream, fileName, "media/server-icons");

        // создаем модель сервера
        var server = Server.Create(Guid.NewGuid(), name, desc, iconPath, isPrivate);

        // добавляем в БД
        await _serverRepository.Add(server, creatorId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetAllServers()
    {

        List<Server> servers = await _serverRepository.GetAllServers();

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetPrivateServers()
    {
        List<Server> servers = await _serverRepository.GetPrivateServers();

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetPublicServers()
    {
        List<Server> servers = await _serverRepository.GetPublicServers();

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
        User user = await _userRepository.GetById(userId);

        if (await _serverRepository.IsMember(serverId, user))
            throw new Exception("Пользователь уже состоит в этом сервере");

        await _serverRepository.AddUser(serverId, user);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
