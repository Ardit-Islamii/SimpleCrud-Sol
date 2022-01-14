using System;
using System.Threading.Tasks;
using InventoryService.Contracts.Repositories;
using InventoryService.Contracts.Services;
using Models;

namespace InventoryService.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task<bool> DecrementItemQuantity(Guid itemId)
        {
            var inventory = await _inventoryRepository.FindByItemID(itemId);
            if (inventory != null)
            {
                if (inventory.Quantity > 0)
                {
                    inventory.Quantity--;
                    return await _inventoryRepository.DecrementItemQuantity(inventory);
                }
            }
            return false;
        }

        public async Task<Inventory> FindByItemID(Guid itemId)
        {
            return await _inventoryRepository.FindByItemID(itemId);
        }

        public async Task<Inventory> Get(Guid Id)
        {
            return await _inventoryRepository.Get(Id);
        }
    }
}
