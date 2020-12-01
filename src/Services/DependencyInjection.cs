using Application.Common.Services;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAgencyService, AgencyService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IImportService<Agent>, ImportAgentsFromZipService>();

            return services;
        }
    }
}