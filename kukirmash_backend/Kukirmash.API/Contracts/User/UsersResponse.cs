namespace Kukirmash.API.Contracts.User;

public record UsersResponse(
    Guid Id,
    string Login,
    string Email
);
