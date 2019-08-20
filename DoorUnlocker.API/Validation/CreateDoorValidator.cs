using DoorUnlocker.API.Models.Offices;
using FluentValidation;

namespace DoorUnlocker.API.Validation
{
    public class CreateDoorValidator : AbstractValidator<CreateOfficeRequest>
    {
        public CreateDoorValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}