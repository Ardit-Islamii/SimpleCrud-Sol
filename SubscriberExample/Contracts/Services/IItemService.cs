using SubscriberExample.Models;
using System;
using System.Threading.Tasks;

namespace SubscriberExample.Contracts.Services
{
    public interface IItemService
    {
        public Task<Item> Create(Item item);
        public bool ExternalItemExists(Guid id);
    }
}
