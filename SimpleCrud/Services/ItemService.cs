using OrderService.Contracts.Repositories;
using OrderService.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace OrderService.Services
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

        public async Task<bool> Delete(Guid Id)
        {
            Item item = await _itemRepository.Get(Id);
            if (item != null)
            {
                return await _itemRepository.Delete(item);
            }
            else
            {
                return false;
            }
        }

        public async Task<Item> Get(Guid Id)
        {
            return await _itemRepository.Get(Id);
        }

        public async Task<List<Item>> Get()
        {
            return await _itemRepository.Get();
        }

        public async Task<Item> Update(Item item)
        {
            var existingItem = await _itemRepository.Get(item.Id);
            if(existingItem != null)
            {
                return await _itemRepository.Update(item);
            }
            else
            {
                return null;
            }
        }
    }
}
