using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Agency
    {
        public Guid Id { get; set; } //= Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactInformation { get; set; }
        public string Status { get; set; }
        public string LegalEntityInformation { get; set; }
        public List<Agent> Agents { get;} = new List<Agent>();
    }
}