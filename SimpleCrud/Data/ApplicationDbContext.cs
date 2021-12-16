using Microsoft.EntityFrameworkCore;
using Models;
using SimpleCrud.Models;

namespace SimpleCrud.Data
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
