using SubscriberExample.Contracts.Repositories;
using SubscriberExample.Data;
using SubscriberExample.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SubscriberExample.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Item> Create(Item item)
        {
            await _context.Items.AddAsync(item);
            _context.SaveChanges();
            return item;
        }

        public bool ExternalItemExists(Guid id)
        {
            return _context.Items.Any(x => x.ExternalId == id);
        }
    }
}
