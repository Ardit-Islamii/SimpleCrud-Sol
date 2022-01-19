using System;
using System.Threading;
using System.Threading.Tasks;
using OrderService.Dtos;
using Refit;

namespace OrderService.DataAccess
{
    /// <summary>
    /// Testing out refit
    /// The endpoint for the InventoryController on InventoryService
    /// </summary>
    public interface IInventoryClientProvider
    {
        /// <summary>
        /// Gets the inventory for an item based on its id.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="cToken"></param>
        /// <returns></returns>
        [Get("/{itemId}")]
        public Task<InventoryReadDto> GetInventory(Guid itemId, CancellationToken cToken = default);
    }
}
