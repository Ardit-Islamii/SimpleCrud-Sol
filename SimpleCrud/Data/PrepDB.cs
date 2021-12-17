using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace OrderService.Data
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        private static void SeedData(ApplicationDbContext context)
        {
            if (!context.Items.Any())
            {
                Console.WriteLine("--> Seeding data");
                context.Items.AddRange(
                        new Item() { Name = "ItemNum1", Price = 100},
                        new Item() { Name = "ItemNum2", Price = 200 },
                        new Item() { Name = "ItemNum3", Price = 300 }
                    );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
