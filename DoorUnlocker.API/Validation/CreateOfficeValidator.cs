using DoorUnlocker.API.Models.Offices;
using FluentValidation;

namespace DoorUnlocker.API.Validation
{
    public class CreateOfficeValidator : AbstractValidator<CreateOfficeRequest>
    {
        public CreateOfficeValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}