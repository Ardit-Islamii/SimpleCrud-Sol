using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace OrderService.Contracts.Repositories
{
    public interface IItemRepository
    {
        public Task<Item> Create(Item entity);

        public Task<Item> Update(Item entity);

        public Task<bool> Delete(Item identity);

        public Task<Item> Get(Guid id);
        public Task<List<Item>> Get();

    }
}
