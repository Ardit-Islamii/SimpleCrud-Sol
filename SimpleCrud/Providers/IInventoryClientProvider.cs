using System;
using System.Threading;
using System.Threading.Tasks;
using Models;
using OrderService.Dtos;
using Refit;

namespace OrderService.DataAccess
{
    public interface IInventoryClientProvider
    {
        [Get("/{itemId}")]
        public Task<InventoryReadDto> GetInventory(Guid itemId, CancellationToken cToken = default);
    }
}
