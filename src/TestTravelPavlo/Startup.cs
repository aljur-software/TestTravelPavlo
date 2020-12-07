using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Infractructure;
using Services;
using TestTravelPavlo.AutoMapper;
using System.Linq;
using System;
using System.IO;

namespace TestTravelPavlo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            CheckOnStartup();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestTravelPavlo", Version = "v1" });
            });
            services.AddInfrastructure(Configuration);
            services.AddServices();
            services.AddAutoMapper(typeof(AgencyProfile).Assembly);
            services.AddMvc().AddFluentValidation(config =>
                config.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestTravelPavlo v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void CheckOnStartup()
        {
            var path = Configuration.GetSection("BulkSettings").GetChildren()
               .FirstOrDefault(_ => _.Key == "FolderForTempCSV")?.Value;

            if (string.IsNullOrEmpty(path))
                throw new Exception("FolderForTempCsv is empty. Indicate FolderForTempCSV in appsettings.json file.");

            if (!Directory.Exists(path))
                throw new Exception($"The Directory {path} is not exist.");
        }
    }
}