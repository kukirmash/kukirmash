using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kukirmash.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly KukirmashDbContext _context;

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

        // Добавление 
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        Log.Information($"Добавлен новый пользователь Id = {user.Id}, Login = {user.Login}, Email = {user.Email}");
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email);

        if (userEntity is null)
            throw new Exception("User not found");

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User> GetByLogin(string login)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == login);

        if (userEntity is null)
            throw new Exception("User not found");

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User> GetById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);

        if (userEntity is null)
            throw new Exception("User not found");

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<User>> GetAllUsers()
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        if (userEntities is null)
            throw new Exception("Users not found");

        List<User> userList = new List<User>();

        foreach (var userEntity in userEntities)
        {
            var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

            userList.Add(user);
        }

        return userList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetUserServers(User user)
    {
        // Получаем сущности серверов, где пользователь числится в списке участников
        var serverEntities = await _context.Servers
            .AsNoTracking()
            .Where(s => s.Users.Any(u => u.Id == user.Id))
            .ToListAsync();

        if (serverEntities is null)
            throw new Exception("User`s servers not found");

        List<Server> servers = serverEntities
            .Select(s => Server.Create(s.Id, s.Name, s.Description))
            .ToList();

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetUserCreatedServers(User user)
    {
        var serverEntities = await _context.Servers
            .AsNoTracking()
            .Where(s => s.CreatorId == user.Id)
            .ToListAsync();

        if (serverEntities is null)
            throw new Exception("User`s servers not found");

        List<Server> servers = serverEntities
            .Select(s => Server.Create(s.Id, s.Name, s.Description))
            .ToList();

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
