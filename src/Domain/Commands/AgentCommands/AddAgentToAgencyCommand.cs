using System;

namespace Domain.Commands.AgentCommands
{
    public class AddAgentToAgencyCommand
    {
        public Guid AgentId { get; set; }
        public Guid AgencyId { get; set; }
    }
}