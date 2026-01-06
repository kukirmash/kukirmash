using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface IServerService
{
    Task Add(Guid creatorId, string name, string desc, Stream iconStream, string fileName);

    Task<List<Server>> GetAllServers();
}
