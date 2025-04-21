using FluentValidation;
using LS.API.Models;

namespace LS.API.FluentValidations
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            // Name must not be empty and have a maximum length of 100 characters.
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters."); ;
        }
    }
}
