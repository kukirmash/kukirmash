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

    //*----------------------------------------------------------------------------------------------------------------------------
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task Register(string login, string email, string password)
    {
        // Проверяем существует ли пользователь с таким же email
        var existingByEmail = await _userRepository.GetByEmail(email);
        if (existingByEmail != null)
            throw new Exception("Email already registered");

        // Проверяем существует ли пользователь с таким же логином
        var existingByLogin = await _userRepository.GetByLogin(login);
        if (existingByLogin != null)
            throw new Exception("Login already registered");

        // Хешируем пароль
        string hashedPassword = _passwordHasher.Generate(password);

        // Создаем пользователя
        var user = User.Create(Guid.NewGuid(), login, email, hashedPassword);

        // Добавляем его в БД
        await _userRepository.Add(user);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<string> LoginByLogin(string login, string password)
    {   
        // Получаем пользовтеля из репозитория
        var user = await _userRepository.GetByLogin(login);
        if (user == null)
            throw new Exception("User with this login is not exsits");

        // Проверяем пароль пользователя
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
            throw new Exception("Password is incorrect");

        // Генерируем jwt токен
        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<string> LoginByEmail(string email, string password)
    {
        // Получаем пользовтеля из репозитория
        var user = await _userRepository.GetByEmail(email);
        if (user == null)
            throw new Exception("User with this email is not exsits");

        // Проверяем пароль пользователя
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
            throw new Exception("Password is incorrect");

        // Генерируем jwt токен
        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
