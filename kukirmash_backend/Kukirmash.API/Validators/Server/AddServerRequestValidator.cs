using System;
using FluentValidation;
using Kukirmash.API.Contracts.Server;

namespace Kukirmash.API.Validators.Server;

public class AddServerRequestValidator : AbstractValidator<AddServerRequest>
{
    public AddServerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название сервера обязательно")
            .MaximumLength(31).WithMessage("Название сервера слишком длинное (макс 31)");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Название сервера слишком длинное (макс 255)");
    }
}
