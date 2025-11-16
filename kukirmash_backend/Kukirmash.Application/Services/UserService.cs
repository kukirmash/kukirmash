using Kukirmash.Application.Interfaces.Auth;
using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;

namespace Kukirmash.Application.Services;

public class UserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    //---------------------------------------------------------------------------
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
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
        var user = await _userRepository.GetByLogin(login);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (result == false)
        {
            throw new Exception("Failed to login");
        }

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    //---------------------------------------------------------------------------
    public async Task<string> LoginByEmail(string email, string password)
    {
        return "";
    }

    //---------------------------------------------------------------------------
}
