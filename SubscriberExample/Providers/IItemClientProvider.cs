using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Refit;

namespace InventoryService.DataAccess
{
    /// <summary>
    /// Testing out refit
    /// All the endpoints for the itemController on OrderService 
    /// </summary>
    public interface IItemClientProvider
    {
        [Get("")]
        public Task<List<Item>> GetItems();

        [Get("/{id}")]
        public Task<Item> GetItem(Guid id);

        [Post("/createitem")]
        public Task<Item> CreateItem([Body] Item item);

        [Put("/updateitem/{id}")]
        public Task<Item> UpdateItem(Guid id,[Body] Item item);

        [Delete("/deleteitem/{id}")]
        public Task<Item> DeleteItem(Guid id);
    }
}
