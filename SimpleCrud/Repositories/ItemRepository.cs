﻿using Microsoft.EntityFrameworkCore;
using SimpleCrud.Contracts.Repositories;
using SimpleCrud.Data;
using SimpleCrud.Models;
using System;
using System.Threading.Tasks;

namespace SimpleCrud.Repositories
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
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> Update(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await Save();
            return item;
        }
        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
