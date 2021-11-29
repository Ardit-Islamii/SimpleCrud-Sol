using SimpleCrud.Models;
using System;
using System.Threading.Tasks;

namespace SimpleCrud.Contracts.Repositories
{
    public interface IItemRepository
    {
        public Task<Item> Create(Item entity);

        public Task<Item> Update(Item entity);

        public Task<bool> Delete(Item identity);

        public Task<Item> Get(Guid id);
    }
}
