using Kukirmash.Application.Interfaces.Auth;
using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;

namespace Kukirmash.Application.Services;

public class UserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    //---------------------------------------------------------------------------
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    //---------------------------------------------------------------------------
    public async Task Register(string login, string email, string password)
    {
        string hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(Guid.NewGuid(), login, email, hashedPassword);

        await _userRepository.Add(user);
    }

    //---------------------------------------------------------------------------
    public async Task<string> LoginByLogin(string login, string password)
    {
        return "";
    }

    //---------------------------------------------------------------------------
    public async Task<string> LoginByEmail(string email, string password)
    {
        return "";
    }

    //---------------------------------------------------------------------------
}
