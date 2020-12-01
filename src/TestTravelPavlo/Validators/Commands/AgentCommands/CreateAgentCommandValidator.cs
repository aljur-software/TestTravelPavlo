using Domain.Commands.AgentCommands;
using FluentValidation;

namespace TestTravelPavlo.Validators.Commands.AgentCommands
{
    public class CreateAgentCommandValidator : AbstractValidator<CreateAgentCommand>
    {
        public CreateAgentCommandValidator()
        {
            RuleFor(_ => _.Name).NotNull().NotEmpty();
        }
    }
}