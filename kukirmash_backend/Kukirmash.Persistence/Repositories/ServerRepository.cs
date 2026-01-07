using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kukirmash.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly KukirmashDbContext _context;

    private const string TAG = "ServerRepository";

    //*----------------------------------------------------------------------------------------------------------------------------
    public ServerRepository(KukirmashDbContext context)
    {
        _context = context;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(Server server, Guid creatorId)
    {
        var creatorEntity = await _context.Users.FindAsync(creatorId);

        if (creatorEntity == null)
            throw new Exception("Creator not found");

        var serverEntity = new ServerEntity()
        {
            Id = server.Id,
            Name = server.Name,
            Description = server.Description,
            CreatorId = creatorId,
            IconPath = server.IconPath,
            Users = new List<UserEntity> { creatorEntity }
        };

        Log.Information("{TAG} - добавление нового сервера... (ID:{ServerId})", TAG, server.Id);

        // Добавление 
        await _context.Servers.AddAsync(serverEntity);
        await _context.SaveChangesAsync();

        Log.Information("{TAG} - сервер добавлен успешно. (ID:{ServerId})", TAG, server.Id);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task AddUser(Server server, User user)
    {
        // 1. Загружаем сервер из базы. 
        var serverEntity = await _context.Servers
            .Include(s => s.Users)
            .FirstOrDefaultAsync(s => s.Id == server.Id);

        if (serverEntity is null)
            throw new Exception("Server not found");

        // 2. Загружаем пользователя из базы
        var userEntity = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userEntity is null)
            throw new Exception("User not found");

        // 3. Проверяем, не состоит ли пользователь уже на сервере, чтобы не было ошибки уникальности
        if (serverEntity.Users.Any(u => u.Id == user.Id))
            throw new Exception($"Пользователь {user.Id} уже является участником сервера {server.Id}");

        Log.Information("{TAG} - добавление нового пользователя на сервер... (UserID:{UserId}  ServerID:{ServerId})", TAG, user.Id, server.Id);

        // 4. Добавляем пользователя в коллекцию сервера
        serverEntity.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        Log.Information("{TAG} - новый пользователь успешно добавлен на сервер... (UserID:{UserId}  ServerID:{ServerId})", TAG, user.Id, server.Id);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetAllServers()
    {
        Log.Information("{TAG} - получение всех существующих серверов...", TAG);

        var serverEntities = await _context.Servers
            .AsNoTracking()
            .ToListAsync();

        Log.Information("{TAG} - успешно получено {cnt} серверов.", TAG, serverEntities.Count);

        if (serverEntities is null)
            throw new Exception("Servers not found");

        List<Server> serverList = new List<Server>();

        foreach (var serverEntity in serverEntities)
        {
            var server = Server.Create(serverEntity.Id,
                                serverEntity.Name,
                                serverEntity.Description,
                                serverEntity.IconPath);

            serverList.Add(server);
        }

        return serverList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public Task<Server> GetById(Guid Id)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public Task<User> GetCreator(Server server)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public Task<List<User>> GetMembers(Server server)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public Task<bool> IsMember(Server server, User user)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
