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
        // Хэшируем пароль
        string passwordHash = _passwordHasher.Generate(password);

        // Создаем модель пользователя
        var user = User.Create(Guid.NewGuid(), login, email, passwordHash);

        await _userRepository.Add(user);
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<string> LoginByLogin(string login, string password)
    {
        // Получаем пользовтеля из репозитория
        var user = await _userRepository.GetByLogin(login);
        if (user == null)
            throw new KeyNotFoundException("Пользователя с данным логином не существует");

        // Проверяем пароль пользователя
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
            throw new InvalidOperationException("Неверный пароль");

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
            throw new KeyNotFoundException("Пользователя с данным email не существует");

        // Проверяем пароль пользователя
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
            throw new InvalidOperationException("Неверный пароль");

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
