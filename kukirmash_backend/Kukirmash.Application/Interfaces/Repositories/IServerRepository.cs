using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface IServerRepository
{
    Task Add(Server server, Guid creatorId, string iconPath);

    Task AddUser(Server server, User user);

    Task<Server> GetById(Guid Id);

    Task<List<Server>> GetAllServers();


    Task<User> GetCreator( Server server );

    Task<List<User>> GetMembers(Server server);

    Task<bool> IsMember( Server server, User user );
}
