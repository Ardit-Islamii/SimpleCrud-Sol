using System;
using System.Threading.Tasks;
using Models;
using OrderService.Dtos;

namespace OrderService.Contracts.Services
{
    public interface IPurchaseService
    {
        public Task<PurchaseReadDto> Create(Guid id);
    }
}
