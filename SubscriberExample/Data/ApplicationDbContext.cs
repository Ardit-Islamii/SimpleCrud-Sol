using Microsoft.EntityFrameworkCore;
using SubscriberExample.Models;

namespace SubscriberExample.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
    }
}
