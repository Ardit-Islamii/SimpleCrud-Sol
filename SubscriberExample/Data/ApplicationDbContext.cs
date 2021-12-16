using Microsoft.EntityFrameworkCore;
using Models;

namespace SubscriberExample.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Inventory> Inventory { get; set; }
    }
}
