using SimpleCrud.Dtos;
using SimpleCrud.Models;

namespace SimpleCrud.AsyncDataServices
{
    public interface IMessageBusClient
    {
        public void PublishNewItem(ItemReadDto item);
    }
}
