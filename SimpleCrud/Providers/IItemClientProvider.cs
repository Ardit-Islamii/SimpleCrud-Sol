using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
using Refit;

namespace OrderService.Providers
{
    public interface IItemClientProvider
    {
        [Post("/testconnection")]
        public Task<ItemReadDto> TestInboundConnection(ItemReadDto itemDto);
    }
}
