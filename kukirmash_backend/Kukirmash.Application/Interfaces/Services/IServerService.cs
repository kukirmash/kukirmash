using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface IServerService
{
    Task Add(Guid creatorId, string name, string desc);

    Task<List<Server>> GetAllServers();
}
