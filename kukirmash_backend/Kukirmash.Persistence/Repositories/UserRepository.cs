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

    //---------------------------------------------------------------------------
    public async Task Add(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };

        Log.Information( $"Добавлен новый пользователь Id = {user.Id}, Login = {user.Login}, Email = {user.Email}, PasswordHash = {user.PasswordHash}" );    

        // Добавление 
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    //---------------------------------------------------------------------------
    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email) ?? throw new Exception();

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //---------------------------------------------------------------------------
    public async Task<User> GetByLogin(string login)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == login) ?? throw new Exception();

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //---------------------------------------------------------------------------
    public async Task<User> GetById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id) ?? throw new Exception();

        var user = User.Create(userEntity.Id,
                                userEntity.Login,
                                userEntity.Email,
                                userEntity.PasswordHash);

        return user;
    }

    //---------------------------------------------------------------------------
}
