using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Models;

namespace InventoryService.Consumers.FaultConsumers
{
    public class PurchaseFaultConsumers : IConsumer<Fault<Purchase>>
    {
        public Task Consume(ConsumeContext<Fault<Purchase>> context)
        {
            Console.WriteLine("A message faulted: {0}\n{1}", context.Message.FaultMessageTypes,context.Message.Exceptions.First().Message);
            return Task.CompletedTask;
        }
    }
}
