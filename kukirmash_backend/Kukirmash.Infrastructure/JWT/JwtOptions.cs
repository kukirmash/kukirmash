using System;

namespace Kukirmash.Infrastructure.JWT;

public class JwtOptions
{
    public string SecretKey { get; set; } = String.Empty;

    public int ExpiresHours { get; set; } = 3600;
}
