using AutoMapper;
using Domain.Commands.AgencyCommands;
using Domain.Entities;

namespace TestTravelPavlo.AutoMapper
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            CreateMap<CreateAgencyCommand, Agency>();
        }
    }
}