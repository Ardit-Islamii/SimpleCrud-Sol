using System;
using GreenPipes;
using InventoryService.Consumers;
using InventoryService.Contracts.Repositories;
using InventoryService.Contracts.Services;
using InventoryService.Data;
using InventoryService.DataAccess;
using InventoryService.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Serilog;
using GreenPipes;
//using SubscriberExample.AsyncDataServices;

namespace InventoryService
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
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Connection-String")));
            services.AddAutoMapper(typeof(Startup));

            /*  Commented out due to transferring to MassTransit instead of using pure RabbitMQ
            services.AddHostedService<MessageBusSubscriber>();
            services.AddSingleton<IEventProcessor, EventProcessor>();*/

            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IInventoryService, Services.InventoryService>();

            
            //MassTransit configuration
            services.AddMassTransit(config =>
            {
                config.AddConsumer<PurchaseConsumer>(config =>
                {
                    config.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)));
                });
                config.SetKebabCaseEndpointNameFormatter();
                config.UsingRabbitMq((ctx, config) =>
                {
                    config.Host("amqp://guest:guest@localhost:5672");
                    config.ConfigureEndpoints(ctx);
                });
            });
            services.AddMassTransitHostedService();

            //Refit configuration
            services.AddRefitClient<IItemClientProvider>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(Configuration["ItemUri"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSerilogRequestLogging();
        }
    }
}
