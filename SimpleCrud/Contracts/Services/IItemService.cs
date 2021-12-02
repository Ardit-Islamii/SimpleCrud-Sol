using SimpleCrud.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleCrud.Contracts.Services
{
    public interface IItemService
    {
        public Task<Item> Create(Item item);
        public Task<Item> Update(Item item);
        public Task<bool> Delete(Guid id);
        public Task<Item> Get(Guid id);
        public Task<List<Item>> Get();

    }
}
