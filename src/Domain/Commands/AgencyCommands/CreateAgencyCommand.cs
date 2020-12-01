namespace Domain.Commands.AgencyCommands
{
    public class CreateAgencyCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactInformation { get; set; }
        public string Status { get; set; }
        public string LegalEntityInformation { get; set; }
    }
}