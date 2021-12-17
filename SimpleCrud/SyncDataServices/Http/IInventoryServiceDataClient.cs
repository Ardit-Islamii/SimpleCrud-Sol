using System.Threading.Tasks;
using OrderService.Dtos;

namespace OrderService.SyncDataServices.Http
{
    public interface IInventoryServiceDataClient
    {
        Task SendItemToSubExample(ItemReadDto item); 
    }
}
