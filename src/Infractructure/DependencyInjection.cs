using Application.Common.Interfaces;
using Domain.Entities;
using Infractructure.BulkData;
using Infractructure.Persistence;
using Infractructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infractructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient(typeof(IRepository<>), typeof(TravelRepository<>));
            services.AddTransient(typeof(IBulkImport<Agency>), typeof(AgencyBulkImport));

            return services;
        }
    }
}