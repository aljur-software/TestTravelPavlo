namespace Domain.Commands.AgentCommands
{
    public class CreateAgentCommand
    {
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactInformation { get; set; }
    }
}