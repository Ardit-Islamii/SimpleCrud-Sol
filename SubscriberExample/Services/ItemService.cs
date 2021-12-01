using SubscriberExample.Contracts.Repositories;
using SubscriberExample.Contracts.Services;
using SubscriberExample.Models;
using System;
using System.Threading.Tasks;

namespace SubscriberExample.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Item> Create(Item item)
        {
            return await _itemRepository.Create(item);
        }

        public bool ExternalItemExists(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
