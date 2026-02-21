using System.ComponentModel.DataAnnotations;

namespace Kukirmash.API.Contracts.User;

public record RegisterUserRequest(
    string Login,
    string Email,
    string Password
);
