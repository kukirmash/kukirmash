using System.Security.Authentication;
using System.Security.Claims;

namespace Kukirmash.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdString = user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        
        Guid id;

        if (Guid.TryParse(userIdString, out id))
            return id;

        throw new AuthenticationException("User ID not found in token");
    }
}
