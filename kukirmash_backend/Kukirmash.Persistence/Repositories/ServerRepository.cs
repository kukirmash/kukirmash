using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Core.Models;

namespace Kukirmash.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly KukirmashDbContext _context;

    public ServerRepository(KukirmashDbContext context)
    {
        _context = context;
    }

    public Task Add(Server server)
    {
        throw new NotImplementedException();
    }

    public Task AddUser(Server server, User user)
    {
        throw new NotImplementedException();
    }

    public Task<List<Server>> GetAllServers()
    {
        throw new NotImplementedException();
    }

    public Task<Server> GetById(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetServerCreator(Server server)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetServerMembers(Server server)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsMemberOfServer(Server server, User user)
    {
        throw new NotImplementedException();
    }
}
