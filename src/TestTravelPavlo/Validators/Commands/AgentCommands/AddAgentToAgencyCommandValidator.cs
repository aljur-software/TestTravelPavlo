using Domain.Commands.AgentCommands;
using FluentValidation;

namespace TestTravelPavlo.Validators.Commands.AgentCommands
{
    public class AddAgentToAgencyCommandValidator : AbstractValidator<AddAgentToAgencyCommand>
    {
        public AddAgentToAgencyCommandValidator()
        {
            RuleFor(_ => _.AgencyId).NotNull().NotEmpty();
            RuleFor(_ => _.AgentId).NotNull().NotEmpty();
        }
    }
}