using System;
using System.Threading.Tasks;
using Models;

namespace OrderService.Contracts.Services
{
    public interface IPurchaseService
    {
        public Task<Purchase> Create(Guid id);
    }
}
