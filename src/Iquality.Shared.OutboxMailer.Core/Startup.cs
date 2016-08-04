using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Iquality.Shared.OutboxMailer.Core.Logging;
using Iquality.Shared.OutboxMailer.Core.Tasks;
using System.Linq;
using Iquality.Shared.OutboxMailer.Core.Models;

namespace Iquality.Shared.OutboxMailer.Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            SerilogConfigurator.Configure(env);

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            // Init DB (apply migrations)
            using (var outboxContext = new OutboxContext()) { outboxContext.Init(); }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            
            services.AddMvc();

            services.AddSwaggerGen();

            // register DB context in OnConfigure method
            //var connection = @"Server=(localdb)\mssqllocaldb;Database=OutboxMailer;Trusted_Connection=True;";
            //services.AddDbContext<OutboxContext>(options => options.UseSqlite());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider sp)
        {
            DependencyResolver.Services = sp;           

            loggerFactory.AddSerilog();

            OutboxProcessor.Startup(new OutboxProcessorSettings
            {
                ItemsPerShot = 10,
                StartTime = DateTime.UtcNow.AddSeconds(10),
                FinishTime = DateTime.UtcNow.AddMinutes(2),
                Frequency = TimeSpan.FromMinutes(1)
            });
            
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();
        }
    }
}
