using Domain.Commands.AgencyCommands;
using FluentValidation;

namespace TestTravelPavlo.Validators.Commands.AgencyCommands
{
    public class CreateAgencyCommandValidator : AbstractValidator<CreateAgencyCommand>
    {
        public CreateAgencyCommandValidator()
        {
            RuleFor(_ => _.Name).NotNull().NotEmpty();
        }
    }
}