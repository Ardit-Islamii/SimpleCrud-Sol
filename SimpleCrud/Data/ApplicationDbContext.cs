using Microsoft.EntityFrameworkCore;
using Models;

namespace OrderService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
    }
}
