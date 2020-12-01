using AutoMapper;
using Domain.Commands.AgentCommands;
using Domain.Entities;

namespace TestTravelPavlo.AutoMapper
{
    public class AgentProfile : Profile
    {
        public AgentProfile()
        {
            CreateMap<CreateAgentCommand, Agent>();
            CreateMap<Agent, CreateAgentCommand>();
        }
    }
}