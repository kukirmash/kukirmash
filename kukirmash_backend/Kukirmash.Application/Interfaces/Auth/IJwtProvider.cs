using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Auth;

public interface IJwtProvider
{
    public string GenerateToken(User user);
}
