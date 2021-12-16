using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;
using SimpleCrud.AsyncDataServices;
using SimpleCrud.Contracts.Repositories;
using SimpleCrud.Contracts.Services;
using SimpleCrud.Data;
using SimpleCrud.DataAccess;
using SimpleCrud.Repositories;
using SimpleCrud.Services;
using SimpleCrud.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
namespace SimpleCrud
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
            /*services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMem"));*/
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpClient<ISubscriberExampleDataClient, HttpSubscriberExampleDataClient>();
            services.AddRefitClient<IInventoryData>().ConfigureHttpClient(cfg =>
            {
                cfg.BaseAddress = new Uri(Configuration["InventoryUri"]);
            });
            /* Commented out due to not being implemented yet.
             * services.AddHostedService<KafkaProducerBackgroundService>();*/
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "SimpleCrud_";
            });

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, config) =>
                {
                    config.Host("amqp://guest:guest@localhost:5672");

                });
            });
            services.AddMassTransitHostedService();
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
            PrepDB.PrepPopulation(app);
        }
    }
}
