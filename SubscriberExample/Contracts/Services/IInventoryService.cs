using System;
using System.Threading.Tasks;
using Models;

namespace InventoryService.Contracts.Services
{
    public interface IInventoryService
    {
        public Task<Inventory> Get(Guid ID);
        public Task<bool> DecrementItemQuantity(Guid itemID);
        public Task<Inventory> FindByItemID(Guid itemID);

    }
}
