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

        Log.Information($"Добавлен новый пользователь Id = {user.Id}, Login = {user.Login}, Email = {user.Email}, PasswordHash = {user.PasswordHash}");

        // Добавление 
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email);

        if (userEntity is null)
            return null;

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash,

                                userEntity.Servers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList(),

                                userEntity.CreatedServers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList());

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User> GetByLogin(string login)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == login);

        if (userEntity is null)
            return null;

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash,

                                userEntity.Servers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList(),

                                userEntity.CreatedServers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList());

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<User> GetById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);

        if (userEntity is null)
            return null;

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash,
                                
                                userEntity.Servers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList(),

                                userEntity.CreatedServers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList());

        return user;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<User>> GetAllUsers()
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        List<User> userList = new List<User>();

        foreach (var userEntity in userEntities)
        {
            userList.Add(User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash,
                                
                                userEntity.Servers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList(),

                                userEntity.CreatedServers.Select(s => Server.Create(
                                    s.Id, s.Name, s.Description, null!, [])).ToList()));
        }

        return userList;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
