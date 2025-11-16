namespace Kukirmash.API.Contracts;

public record UserResponse(
    Guid Id,
    string Login,
    string Email,
    string PasswordHash
);
