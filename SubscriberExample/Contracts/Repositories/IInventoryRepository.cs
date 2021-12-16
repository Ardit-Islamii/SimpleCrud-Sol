using Models;
using System;
using System.Threading.Tasks;

namespace SubscriberExample.Contracts.Repositories
{
    public interface IInventoryRepository
    {
        public Task<Inventory> Get(Guid ID);
        public Task<Inventory> FindByItemID(Guid itemID);
        public Task<bool> DecrementItemQuantity(Inventory entity);
    }
}
