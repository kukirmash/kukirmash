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
            IsPrivate = server.IsPrivate,
            Users = new List<UserEntity> { creatorEntity }
        };

        Log.Information("{TAG} - добавление нового сервера... (ID:{ServerId})", TAG, server.Id);

        // Добавление 
        await _context.Servers.AddAsync(serverEntity);
        await _context.SaveChangesAsync();

        Log.Information("{TAG} - сервер добавлен успешно. (ID:{ServerId})", TAG, server.Id);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task AddUser(Guid serverId, User user)
    {
        // 1. Загружаем сервер из базы. 
        var serverEntity = await _context.Servers
            .Include(s => s.Users)
            .FirstOrDefaultAsync(s => s.Id == serverId);

        if (serverEntity is null)
            throw new Exception("Сервер не найден");

        // 2. Загружаем пользователя из базы
        var userEntity = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userEntity is null)
            throw new Exception("Пользователь найден");

        // 3. Проверяем, не состоит ли пользователь уже на сервере, чтобы не было ошибки уникальности
        if (serverEntity.Users.Any(u => u.Id == user.Id))
            throw new Exception($"Пользователь {user.Id} уже является участником сервера {serverId}");

        Log.Information("{TAG} - добавление нового пользователя на сервер... (UserID:{UserId}  ServerID:{ServerId})", TAG, user.Id, serverId);

        // 4. Добавляем пользователя в коллекцию сервера
        serverEntity.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        Log.Information("{TAG} - новый пользователь успешно добавлен на сервер... (UserID:{UserId}  ServerID:{ServerId})", TAG, user.Id, serverId);
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
                                serverEntity.IconPath,
                                serverEntity.IsPrivate);

            serverList.Add(server);
        }

        return serverList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------

    public async Task<List<Server>> GetPublicServers()
    {
        Log.Information("{TAG} - получение всех публичных серверов...", TAG);

        var serverEntities = await _context.Servers
            .AsNoTracking()
            .Where(s => s.IsPrivate == false)
            .ToListAsync();

        Log.Information("{TAG} - успешно получено {cnt} публичных серверов.", TAG, serverEntities.Count);

        if (serverEntities is null)
            throw new Exception("Публичные сереверы не были найдены");

        List<Server> serverList = new List<Server>();

        foreach (var serverEntity in serverEntities)
        {
            var server = Server.Create(serverEntity.Id,
                                serverEntity.Name,
                                serverEntity.Description,
                                serverEntity.IconPath,
                                serverEntity.IsPrivate);

            serverList.Add(server);
        }

        return serverList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------

    public async Task<List<Server>> GetPrivateServers()
    {
        Log.Information("{TAG} - получение всех приватных серверов...", TAG);

        var serverEntities = await _context.Servers
            .AsNoTracking()
            .Where(s => s.IsPrivate == true)
            .ToListAsync();

        Log.Information("{TAG} - успешно получено {cnt} приватных серверов.", TAG, serverEntities.Count);

        if (serverEntities is null)
            throw new Exception("Приватные сереверы не были найдены");

        List<Server> serverList = new List<Server>();

        foreach (var serverEntity in serverEntities)
        {
            var server = Server.Create(serverEntity.Id,
                                serverEntity.Name,
                                serverEntity.Description,
                                serverEntity.IconPath,
                                serverEntity.IsPrivate);

            serverList.Add(server);
        }

        return serverList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<Server> GetById(Guid Id)
    {
        var serverEntity = await _context.Servers
            .FindAsync(Id);

        if (serverEntity is null)
            return null;

        var server = Server.Create(serverEntity.Id,
                                serverEntity.Name,
                                serverEntity.Description,
                                serverEntity.IconPath,
                                serverEntity.IsPrivate);

        return server;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public Task<User> GetCreator(Guid serverId)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<User>> GetMembers(Guid serverId)
    {
        var serverEntity = await _context.Servers
        .Include(s => s.Users) // <--- ВАЖНО: Подгружаем пользователей
        .FirstOrDefaultAsync(s => s.Id == serverId);

        if (serverEntity is null)
            return null;

        return serverEntity.Users
            .Select(u => User.Create(u.Id, u.Login, u.Email, u.PasswordHash))
            .ToList();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<bool> IsMember(Guid serverId, User user)
    {
        var serverEntity = await _context.Servers
                .Include(s => s.Users) // <--- ВАЖНО: Подгружаем пользователей
                .FirstOrDefaultAsync(s => s.Id == serverId);

        if (serverEntity is null)
            return false;

        foreach (var u in serverEntity.Users)
        {
            if (u.Id == user.Id)
                return true;
        }

        return false;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
