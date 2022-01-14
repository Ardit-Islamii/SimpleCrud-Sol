using OrderService.Dtos;

namespace OrderService.AsyncDataServices
{
    //Unused code due to using MassTransit instead of pure RabbitMQ
    public interface IMessageBusClient
    {
        public void PublishNewItem(ItemReadDto item);
    }
}
