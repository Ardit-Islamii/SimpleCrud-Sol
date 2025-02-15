﻿using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Contracts.Repositories;
using InventoryService.Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace InventoryService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DecrementItemQuantity(Inventory entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<Inventory> FindByItemID(Guid itemId)
        {
            return await _context.Inventory.Where(x => x.ItemId == itemId).FirstOrDefaultAsync();
        }

        public async Task<Inventory> Get(Guid id)
        {
            return await _context.Inventory.FindAsync(id);
        }
    }
}
