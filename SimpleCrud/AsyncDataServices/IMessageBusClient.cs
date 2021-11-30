using SimpleCrud.Models;

namespace SimpleCrud.AsyncDataServices
{
    public interface IMessageBusClient
    {
        public void PublishNewItem(Item item);
    }
}
