using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Agent
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? ContactInformation { get; set; }

    }
}
