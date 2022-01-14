using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GreenPipes;
using InventoryService.Consumers;
using InventoryService.Contracts.Services;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using UnitTesting.Helper;
using Xunit;

namespace UnitTesting.PurchaseConsumerUnitTest
{
    public class ConsumerUnitTest
    {
        private readonly Purchase purchaseItem = PurchaseHelper.PurchaseData();
        private readonly Mock<IInventoryService> _mockedInventoryService;
        private readonly Mock<ILogger<PurchaseConsumer>> _mockedLogger;

        public ConsumerUnitTest()
        {
            _mockedInventoryService = new Mock<IInventoryService>();
            _mockedLogger = new Mock<ILogger<PurchaseConsumer>>();

        }
        [Fact]
        public async Task PublishPurchase_()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer<PurchaseConsumer>(() => new PurchaseConsumer(_mockedLogger.Object, _mockedInventoryService.Object),cfg =>
            {
                cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2)));
            });
            
            await harness.Start();

            try
            {
                //Arrange
                _mockedInventoryService.Setup(x => x.DecrementItemQuantity(It.IsAny<Guid>())).ReturnsAsync(true);

                //Act
                await harness.InputQueueSendEndpoint.Send<Purchase>(purchaseItem);

                //Assert
                // did the endpoint consume the message
                Assert.True(await harness.Consumed.Any<Purchase>());

                // did the actual consumer consume the message
                Assert.True(await consumerHarness.Consumed.Any<Purchase>());

            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task PublishPurchase_FirstTry_FaultPublished()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer<PurchaseConsumer>(() => new PurchaseConsumer(_mockedLogger.Object, _mockedInventoryService.Object), cfg =>
            {
                cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2)));
            });

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send<Purchase>(purchaseItem);
                
                // did the endpoint consume the message
                Assert.True(await harness.Consumed.Any<Purchase>());

                // did the actual consumer consume the message
                Assert.True(await consumerHarness.Consumed.Any<Purchase>());

                // ensure that no faults were published by the consumer
                Assert.True(await harness.Published.Any<Fault<Purchase>>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
