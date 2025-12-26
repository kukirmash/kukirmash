using Kukirmash.Core.Models;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Interfaces.Repositories;

namespace Kukirmash.Application.Services;

public class ServerService : IServerService
{
    private readonly IServerRepository _serverRepository;

    public ServerService(IServerRepository serverRepository)
    {
        _serverRepository = serverRepository;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(Guid creatorId, string name, string desc)
    {
        // Пока нет проверок. Возможные проверки:
        // 1)

        // создаем модель сервера
        var server = Server.Create(Guid.NewGuid(), name, desc);

        // добавляем в БД
        await _serverRepository.Add(server, creatorId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetAllServers()
    {

        List<Server> servers = await _serverRepository.GetAllServers();

        return servers;
    }
}
