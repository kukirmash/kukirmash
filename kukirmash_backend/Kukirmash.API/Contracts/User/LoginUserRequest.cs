using System;
using System.ComponentModel.DataAnnotations;

namespace Kukirmash.API.Contracts.User;

public record LoginUserRequest(
    [Required] string Login,
    [Required] string Password
    );

