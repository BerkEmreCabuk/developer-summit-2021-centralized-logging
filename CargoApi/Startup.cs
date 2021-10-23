using DeveloperSummit.Core.Extensions;
using DeveloperSummit.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DeveloperSummit.CargoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.json", reloadOnChange: false, optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: false, optional: true)
                .Build(); ;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RabbitMqConfigModel rabbitMqConfigModel = new RabbitMqConfigModel();
            Configuration.GetSection("RabbitMqConfig").Bind(rabbitMqConfigModel);
            services.Configure<RabbitMqConfigModel>(Configuration.GetSection("RabbitMqConfig"));
            services.AddControllers();
            services.AddCustomHttpClient();
            services.AddRabbitMq(rabbitMqConfigModel);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCustomLogManager();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
