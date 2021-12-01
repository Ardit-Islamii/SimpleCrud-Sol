using SubscriberExample.Models;
using System;
using System.Threading.Tasks;

namespace SubscriberExample.Contracts.Repositories
{
    public interface IItemRepository
    {
        public Task<Item> Create(Item item);
        public bool ExternalItemExists(Guid id);
    }
}
