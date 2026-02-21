using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kukirmash.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly KukirmashDbContext _context;

    private const string TAG = "ServerRepository";

    //*----------------------------------------------------------------------------------------------------------------------------
    public ServerRepository(KukirmashDbContext context)
    {
        _context = context;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(Server server)
    {
        var creatorEntity = await _context.Users
            .FindAsync(server.CreatorId);

        if (creatorEntity is null)
            throw new KeyNotFoundException($"Пользователь с ID {server.CreatorId} не найден");

        var serverEntity = new ServerEntity()
        {
            Id = server.Id,
            Name = server.Name,
            Description = server.Description,
            CreatorId = server.CreatorId,
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
    public async Task AddUser(Guid serverId, Guid userId)
    {
        Log.Information("{TAG} - добавление нового пользователя на сервер... (UserID:{UserId}  ServerID:{ServerId})", TAG, userId, serverId);

        // 1. Загружаем сервер из базы. 
        var serverEntity = await _context.Servers
            .FindAsync(serverId);

        if (serverEntity is null)
        {
            Log.Information("{TAG} - Сервер с ID {serverId} не найден", TAG, serverId);
            throw new KeyNotFoundException("Сервер не найден");
        }
        // 2. Загружаем пользователя из базы
        var userEntity = await _context.Users
            .FindAsync(userId);

        if (userEntity is null)
        {
            Log.Information("{TAG} - Пользователь с ID {userId} не найден", TAG, userId);
            throw new KeyNotFoundException("Пользователь не найден");
        }

        // 3. Проверяем, не состоит ли пользователь уже на сервере, чтобы не было ошибки уникальности
        if (serverEntity.Users.Any(u => u.Id == userId))
        {
            Log.Information("{TAG} - Пользователь с ID {userId} уже состоит в этом сервере", TAG, userId);
            throw new InvalidOperationException("Пользователь уже состоит в этом сервере");
        }

        // 4. Добавляем пользователя в коллекцию сервера
        serverEntity.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        Log.Information("{TAG} - новый пользователь успешно добавлен на сервер... (UserID:{UserId}  ServerID:{ServerId})", TAG, userId, serverId);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<bool> IsMember(Guid serverId, Guid userId)
    {
        return await _context.Servers
                .AsNoTracking()
                .AnyAsync(s => s.Id == serverId && s.Users.Any(u => u.Id == userId));
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetAllServers(bool? isPrivate = null)
    {
        Log.Information("{TAG} - получение всех серверов...", TAG);

        var serverEntities = _context.Servers.AsNoTracking();

        if (isPrivate != null)
            serverEntities = serverEntities.Where(s => s.IsPrivate == isPrivate.Value);

        var serverEntitiesList = await serverEntities.ToListAsync();

        Log.Information("{TAG} - успешно получено {cnt} серверов.", TAG, serverEntitiesList.Count);

        List<Server> serverList = serverEntitiesList
            .Select(s => Server.Create(s.Id, s.Name, s.Description, s.IconPath, s.IsPrivate, s.CreatorId))
            .ToList();

        return serverList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<Server?> GetById(Guid Id)
    {
        var serverEntity = await _context.Servers
            .FindAsync(Id);

        if (serverEntity is null)
            return null;

        var server = Server.Create(serverEntity.Id,
                                serverEntity.Name,
                                serverEntity.Description,
                                serverEntity.IconPath,
                                serverEntity.IsPrivate,
                                serverEntity.CreatorId);

        return server;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User?> GetCreator(Guid serverId)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<User>> GetMembers(Guid serverId)
    {
        var serverEntity = await _context.Servers
        .AsNoTracking()
        .Include(s => s.Users) // Подгружаем пользователей
        .FirstOrDefaultAsync(s => s.Id == serverId);

        if (serverEntity is null)
            throw new KeyNotFoundException("Сервер не найден");

        return serverEntity.Users
            .Select(u => User.Create(u.Id, u.Login, u.Email, u.PasswordHash))
            .ToList();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<TextChannel>> GetTextChannels(Guid serverId)
    {
        var serverEntity = await _context.Servers
            .FindAsync(serverId);

        if (serverEntity is null)
            throw new KeyNotFoundException($"Сервер с ID {serverId} не найден");

        var textChannelEntities = await _context.TextChannels
            .Where(tch => tch.ServerId == serverId)
            .ToListAsync();

        List<TextChannel> textChannels = new List<TextChannel>();

        foreach (var textChannelEntity in textChannelEntities)
        {
            TextChannel textChannel = TextChannel.Create(textChannelEntity.Id, textChannelEntity.Name, serverId);
            textChannels.Add(textChannel);
        }

        return textChannels;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<bool> HasTextChannel(Guid serverId, Guid textChannelId)
    {
        return await _context.Servers
                .AsNoTracking()
                .AnyAsync(s => s.Id == serverId && s.TextChannels.Any(tch => tch.Id == textChannelId));
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
