using System;
using System.ComponentModel.DataAnnotations;

namespace Kukirmash.API.Contracts.User;

public record LoginUserRequest(
    string Login,
    string Password
    );

