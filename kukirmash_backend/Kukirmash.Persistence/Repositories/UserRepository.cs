using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;

namespace Kukirmash.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly KukirmashDbContext _context;
    private const string TAG = "UserRepository";
    public UserRepository(KukirmashDbContext context)
    {
        _context = context;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Add(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };

        Log.Information("{TAG}: добавление нового пользователя... (Логин:{Login})", TAG, user.Login);

        // Добавление 
        try
        {
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Проверяем, что это ошибка уникальности Postgres (код 23505)
            if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                Log.Information("{TAG}: пользователь с таким логином или email уже существует. (Логин:{Login})", TAG, user.Login);
                // Выбрасываем обычное исключение, которое не требует библиотек БД
                throw new InvalidOperationException("Пользователь с таким логином или email уже существует");
            }

            // Если это другая ошибка - пробрасываем её дальше
            throw;
        }

        Log.Information("{TAG}: новый пользователь добавлен успешно. (Логин:{Login})", TAG, user.Login);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User?> GetByEmail(string email)
    {
        Log.Information("{TAG}: поиск пользователя... (Email:{email})", TAG, email);

        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email);

        if (userEntity is null)
        {
            Log.Information("{TAG}: данного пользователя не существует. (Email:{email})", TAG, email);
            return null;
        }
        Log.Information("{TAG}: пользователь успешно найден. (Email:{email})", TAG, email);

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User?> GetByLogin(string login)
    {
        Log.Information("{TAG}: поиск пользователя... (Login:{login})", TAG, login);

        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == login);

        if (userEntity is null)
        {
            Log.Information("{TAG}: данного пользователя не существует. (Login:{login})", TAG, login);
            return null;
        }
        Log.Information("{TAG}: пользователь успешно найден. (Login:{login})", TAG, login);

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User?> GetById(Guid id)
    {
        Log.Information("{TAG}: поиск пользователя... (id:{id})", TAG, id);
        var userEntity = await _context.Users
            .FindAsync(id);

        if (userEntity is null)
        {
            Log.Information("{TAG}: данного пользователя не существует. (id:{id})", TAG, id);
            return null;

        }
        Log.Information("{TAG}: пользователь успешно найден. (id:{id})", TAG, id);

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<User>> GetAllUsers()
    {
        Log.Information("{TAG}: получение всех пользователей...", TAG);

        var userEntities = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        Log.Information("{TAG}: получены {Count} пользователей.", TAG, userEntities.Count);

        List<User> userList = userEntities
            .Select(u => User.Create(u.Id, u.Login, u.Email, u.PasswordHash))
            .ToList();

        return userList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetUserServers(Guid userId)
    {
        Log.Information("{TAG}: получение всех серверов, где пользователь участник... (UserId:{userId})", TAG, userId);

        // Получаем сущности серверов, где пользователь числится в списке участников
        var serverEntities = await _context.Servers
            .AsNoTracking()
            .Where(s => s.Users.Any(u => u.Id == userId))
            .ToListAsync();

        Log.Information("{TAG}: успешно получено {Count} серверов, где пользователь участник... (UserId:{userId})", TAG, serverEntities.Count, userId);

        List<Server> servers = serverEntities
            .Select(s => Server.Create(s.Id, s.Name, s.Description, s.IconPath, s.IsPrivate, s.CreatorId))
            .ToList();

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetUserCreatedServers(Guid userId)
    {
        Log.Information("{TAG}: получение всех серверов, которые создал пользователь... (UserId:{userId})", TAG, userId);

        var serverEntities = await _context.Servers
            .AsNoTracking()
            .Where(s => s.CreatorId == userId)
            .ToListAsync();

        Log.Information("{TAG}: успешно получено {Count} серверов, которые создал пользователь... (UserId:{userId})", TAG, serverEntities.Count, userId);

        List<Server> servers = serverEntities
            .Select(s => Server.Create(s.Id, s.Name, s.Description, s.IconPath, s.IsPrivate, s.CreatorId))
            .ToList();

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
