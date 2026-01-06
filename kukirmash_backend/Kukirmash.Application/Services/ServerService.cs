using Kukirmash.Core.Models;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces;

namespace Kukirmash.Application.Services;

public class ServerService : IServerService
{
    private readonly IServerRepository _serverRepository;
    private readonly IStaticFileService _staticFileService;

    public ServerService(IServerRepository serverRepository, IStaticFileService staticFileService)
    {
        _serverRepository = serverRepository;
        _staticFileService = staticFileService;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(Guid creatorId, string name, string desc, Stream iconStream, string fileName)
    {
        // Пока нет проверок. Возможные проверки:
        // 1)

        string iconPath = null;
        if (iconStream != null && fileName != null)
        {
            iconPath = await _staticFileService.UploadFile(iconStream, fileName, "media/server-icons");
        }

        // создаем модель сервера
        var server = Server.Create(Guid.NewGuid(), name, desc);

        // добавляем в БД
        await _serverRepository.Add(server, creatorId, iconPath);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetAllServers()
    {

        List<Server> servers = await _serverRepository.GetAllServers();

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
