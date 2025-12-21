using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task Add(User user);

    Task<User> GetByEmail(string email);

    Task<User> GetByLogin(string login);

    Task<User> GetById(Guid id);

    Task<List<User>> GetAllUsers();


    Task<List<Server>> GetUserServers(User user);

    Task<List<Server>> GetUserCreatedServers(User user);
}
