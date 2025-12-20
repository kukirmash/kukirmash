using System;
using Kukirmash.Core.Models;

namespace Kukirmash.Persistence.Repositories;

public class ServerRepository
{
    private readonly KukirmashDbContext _context;

    public ServerRepository(KukirmashDbContext context)
    {
        _context = context;
    }


    public async Task Add( Server server )
    {

    }

    public async Task AddUser( Server server, User user )
    {
        
    }

    public async Task<Server> GetById(Guid Id)
    {
        return null;
    }

    public async Task<List<Server>> GetAllServers()
    {
        return null;
    }
}
