using OrderService.Dtos;

namespace OrderService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        public void PublishNewItem(ItemReadDto item);
    }
}
