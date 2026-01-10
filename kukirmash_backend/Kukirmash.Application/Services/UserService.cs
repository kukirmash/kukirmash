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
        // Проверяем существует ли пользователь с таким же логином
        var existingByLogin = await _userRepository.GetByLogin(login);

        // Генерируем ошибки в зависимости от занятости 
        if (existingByEmail != null && existingByLogin != null)
            throw new Exception("Пользователь с данным email и логином уже существует");

        if (existingByEmail != null)
            throw new Exception("Пользователь с данным email уже существует");

        if (existingByLogin != null)
            throw new Exception("Пользователь с данным логином уже существует");

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
            throw new Exception("Пользователя с данным логином не существует");

        // Проверяем пароль пользователя
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
            throw new Exception("Неверный пароль");

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
            throw new Exception("Пользователя с данным email не существует");

        // Проверяем пароль пользователя
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
            throw new Exception("Неверный пароль");

        // Генерируем jwt токен
        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<User>> GetAllUsers()
    {
        List<User> users = await _userRepository.GetAllUsers();

        return users;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetUserServers(Guid userId)
    {
        List<Server> servers = await _userRepository.GetUserServers(userId);

        return servers;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<List<Server>> GetUserCreatedServers(Guid userId)
    {
        List<Server> serversCreated = await _userRepository.GetUserCreatedServers(userId);

        return serversCreated;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
