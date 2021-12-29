using System;
using Elasticsearch.Net;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Nest;
using OrderService.Contracts.Repositories;
using OrderService.Contracts.Services;
using OrderService.Data;
using OrderService.DataAccess;
using OrderService.Repositories;
using OrderService.Services;
using OrderService.SyncDataServices.Http;
using Refit;
using Serilog;

namespace OrderService
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
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddSingleton<IElasticClient>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var settings = new ConnectionSettings(config["cloudId"],
                    new BasicAuthenticationCredentials("elastic", config["password"]))
                    .DefaultIndex("an-example-index")
                    .DefaultMappingFor<Purchase>(i => i.IndexName("purchase-demo-v1"));
                return new ElasticClient(settings);
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpClient<IInventoryServiceDataClient, InventoryServiceDataClient>();
            services.AddRefitClient<IInventoryClientProvider>().ConfigureHttpClient(cfg =>
            {
                cfg.BaseAddress = new Uri(Configuration["InventoryUri"]);
            });

            /* Commented out due to transferring to MassTransit instead of pure RabbitMQ
             * services.AddSingleton<IMessageBusClient, MessageBusClient>();*/

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
