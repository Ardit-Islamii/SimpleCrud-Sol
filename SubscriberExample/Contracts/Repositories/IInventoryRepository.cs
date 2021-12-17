using System;
using System.Threading.Tasks;
using Models;

namespace InventoryService.Contracts.Repositories
{
    public interface IInventoryRepository
    {
        public Task<Inventory> Get(Guid ID);
        public Task<Inventory> FindByItemID(Guid itemID);
        public Task<bool> DecrementItemQuantity(Inventory entity);
    }
}
