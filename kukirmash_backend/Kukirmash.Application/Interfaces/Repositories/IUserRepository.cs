using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task Add(User user);

    Task<User> GetByEmail(string email);

    Task<User> GetByLogin(string login);

    Task<List<User>> GetAllUsers();
}
