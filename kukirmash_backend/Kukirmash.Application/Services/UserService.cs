using Kukirmash.Application.Exceptions;
using Kukirmash.Application.Interfaces.Auth;
using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Core.Models;

namespace Kukirmash.Application.Services;

public class UserService : IUserService
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
        // Проверяем существует ли пользователь с таким же email
        var existingByEmail = await _userRepository.GetByEmail(email);
        if (existingByEmail != null)
            throw new UserAlreadyExistsException("Email already registered");

        // Проверяем существует ли пользователь с таким же логином
        var existingByLogin = await _userRepository.GetByLogin(login);
        if (existingByLogin != null)
            throw new UserAlreadyExistsException("Login already registered");

        // Хешируем пароль
        string hashedPassword = _passwordHasher.Generate(password);

        // Создаем пользователя
        var user = User.Create(Guid.NewGuid(), login, email, hashedPassword);

        // Добавляем его в БД
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
