using System.Threading.Tasks;
using OrderService.Dtos;
using Refit;

namespace OrderService.Providers
{
    public interface IItemClientProvider
    {
        /// <summary>
        /// Sends an ItemReadDto to the inventoryService on the itemController testinboundconnection endpoint, logs that it worked correctly and sends the dto back.
        /// </summary>
        /// <param name="itemDto"></param>
        /// <returns></returns>
        [Post("/testconnection")]
        public Task<ItemReadDto> TestInboundConnection(ItemReadDto itemDto);
    }
}
