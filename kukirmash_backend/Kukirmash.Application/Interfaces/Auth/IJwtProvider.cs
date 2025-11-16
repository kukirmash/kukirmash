using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
