using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface IServerRepository
{
    Task Add(Server server);

    Task AddUser(Server server, User user);

    Task<Server> GetById(Guid Id);

    Task<List<Server>> GetAllServers();


    Task<User> GetServerCreator( Server server );

    Task<List<User>> GetServerMembers(Server server);

    Task<bool> IsMemberOfServer( Server server, User user );
}
