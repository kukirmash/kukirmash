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

    public ServerService(IServerRepository serverRepository, IStaticFileService staticFileService)
    {
        _serverRepository = serverRepository;
        _staticFileService = staticFileService;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // Добавляет сервер без фото
    public async Task Add(Guid creatorId, string name, string? desc)
    {
        // создаем модель сервера без фото
        var server = Server.Create(Guid.NewGuid(), name, desc, null);

        // добавляем в БД
        await _serverRepository.Add(server, creatorId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    // Добавляет сервер с фото
    public async Task Add(Guid creatorId, string name, string? desc, Stream iconStream, string fileName)
    {
        // Пока нет проверок.
        // 1) Пользователь может создавать ограниченного колво серверов

        // Сохраняем фото -> отностельный путь где он сохранен, в папке wwwroot
        string iconPath = await _staticFileService.UploadFile(iconStream, fileName, "media/server-icons");

        // создаем модель сервера
        var server = Server.Create(Guid.NewGuid(), name, desc, iconPath);

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
    public Task<string> GetServerInviteToken(Guid serverId)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
