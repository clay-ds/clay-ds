using DoorUnlocker.API.Models.Entrance;
using FluentValidation;

namespace DoorUnlocker.API.Validation
{
    public class GetHistoryValidator : AbstractValidator<GetHistoryRequest>
    {
        public GetHistoryValidator()
        {
            RuleFor(r => r.Count)
                .GreaterThan(0)
                .LessThanOrEqualTo(50);
        }
    }
}