using Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubscriberExample.DataAccess
{
    public interface IItemData
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
