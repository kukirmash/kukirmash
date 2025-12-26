using System.ComponentModel.DataAnnotations;

namespace Kukirmash.API.Contracts.User;

public record RegisterUserRequest(
    [Required] string Login,
    [Required] string Email,
    [Required] string Password
);
