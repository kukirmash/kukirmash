using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kukirmash.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly KukirmashDbContext _context;

    //*----------------------------------------------------------------------------------------------------------------------------
    public ServerRepository(KukirmashDbContext context)
    {
        _context = context;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(Server server, Guid creatorId)
    {
        var serverEntity = new ServerEntity()
        {
            Id = server.Id,
            Name = server.Name,
            Description = server.Description,
            CreatorId = creatorId
        };

        // Добавление 
        await _context.Servers.AddAsync(serverEntity);
        await _context.SaveChangesAsync();

        Log.Information($"Добавлен новый сервер Id = {server.Id}, Name = {server.Name}");
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task AddUser(Server server, User user)
    {
        // 1. Загружаем сервер из базы. 
        var serverEntity = await  _context.Servers
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

        // 4. Добавляем пользователя в коллекцию сервера
        serverEntity.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        Log.Information($"Пользователь {user.Login} успешно добавлен на сервер {server.Name}");
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetAllServers()
    {
        var serverEntities = await _context.Servers
            .AsNoTracking()
            .ToListAsync();

        if (serverEntities is null)
            throw new Exception("Servers not found");

        List<Server> serverList = new List<Server>();

        foreach (var serverEntity in serverEntities)
        {
            var server = Server.Create(serverEntity.Id,
                                serverEntity.Name,
                                serverEntity.Description);

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
