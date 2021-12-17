using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using OrderService.Contracts.Repositories;
using OrderService.Data;

namespace OrderService.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Item> Create(Item item)
        {
            await _context.Items.AddAsync(item);
            await Save();
            return item;
        }

        public async Task<bool> Delete(Item item)
        {
            _context.Items.Remove(item);
            return await Save();
        }

        public async Task<Item> Get(Guid id)
        {
            Item item = await _context.Items.FindAsync(id);
            _context.Entry(item).State = EntityState.Detached;
            return item;
        }

        public async Task<Item> Update(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await Save();
            return item;
        }
        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Item>> Get()
        {
            return await _context.Items.ToListAsync();  
        }
    }
}
