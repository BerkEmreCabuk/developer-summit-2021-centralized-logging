using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperSummit.Core.Extensions;
using DeveloperSummit.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DeveloperSummit.InvoiceApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
