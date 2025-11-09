using Kukirmash.Core.Models;
using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;

namespace Kukirmash.Persistence.Repositories;

public class UserRepository
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
}
