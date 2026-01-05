using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Services;

public interface IServerService
{
    Task Add(Guid creatorId, string name, string desc, string iconPath);

    Task<List<Server>> GetAllServers();
}
