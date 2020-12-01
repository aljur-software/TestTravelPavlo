using Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAgencyService, AgencyService>();

            return services;
        }
    }
}
