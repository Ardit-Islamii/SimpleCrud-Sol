using SimpleCrud.Dtos;
using System.Threading.Tasks;

namespace SimpleCrud.SyncDataServices.Http
{
    public interface ISubscriberExampleDataClient
    {
        Task SendItemToSubExample(ItemReadDto item); 
    }
}
