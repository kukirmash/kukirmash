using System;
using FluentValidation;
using Kukirmash.API.Contracts.User;

namespace Kukirmash.API.Validators.User;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        // может быть как логин так и почта
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Логин обязателен");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(8).WithMessage("Пароль должен быть не короче 8 символов");
    }
}
