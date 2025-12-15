namespace Kukirmash.API.Contracts;

public record UsersResponse(
    Guid Id,
    string Login,
    string Email
);
