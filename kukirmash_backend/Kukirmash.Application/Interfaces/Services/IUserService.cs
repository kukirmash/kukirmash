using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface IUserService
{
    Task<string> LoginByEmail(string email, string password);
    Task<string> LoginByLogin(string login, string password);
    Task Register(string login, string email, string password);
    Task<List<User>> GetAllUsers();

    Task<List<Server>> GetUserServers(Guid userId);
    Task<List<Server>> GetUserCreatedServers(Guid userId);
}