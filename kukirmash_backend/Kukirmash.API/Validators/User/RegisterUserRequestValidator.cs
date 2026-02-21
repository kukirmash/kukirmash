using FluentValidation;
using Kukirmash.API.Contracts.User;

namespace Kukirmash.API.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Логин обязателен")
            .Length(3, 31).WithMessage("Логин должен быть от 3 до 31 символов")
            .Matches("^[a-zA-Z0-9_]*$").WithMessage("Логин может содержать только буквы и цифры");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Почта обязательна")
            .EmailAddress().WithMessage("Некорректный Email");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(8).WithMessage("Пароль должен быть не короче 8 символов");
    }
}
