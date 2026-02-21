using FluentValidation;
using Kukirmash.API.Contracts.TextChannel;

namespace Kukirmash.API.Validators.TextChannel;

public class AddTextChannelRequestValidator : AbstractValidator<AddTextChannelRequest>
{
    public AddTextChannelRequestValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Название текстового канала обязательно")
        .MaximumLength(31).WithMessage("Название текстового канала слишком длинное");
    }
}
