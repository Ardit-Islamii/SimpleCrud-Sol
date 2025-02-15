﻿using System.Threading.Tasks;
using Models;
using OrderService.Contracts.Repositories;
using OrderService.Data;

namespace OrderService.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Purchase> Create(Purchase entity)
        {
            await _context.Purchases.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
