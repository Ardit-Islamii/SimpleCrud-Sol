/*

  Commented out due to transferring to MassTransit. 

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using SubscriberExample.Contracts.Repositories;
using SubscriberExample.Contracts.Services;
using SubscriberExample.Dtos;
using SubscriberExample.Models;
using System;
using System.Text.Json;

namespace SubscriberExample.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, ILoggerFactory logger, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _logger = logger.CreateLogger("EventProcessorLogger");
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.ItemPublished:
                    addItem(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "Item_Published":
                    _logger.LogInformation("--> Item published event detected");
                    return EventType.ItemPublished;
                default:
                    _logger.LogInformation("--> Could not determine event type");
                    return EventType.Undetermined;
            }
        }

        private void addItem(string itemPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                var itemPublishedDto = JsonSerializer.Deserialize<ItemPublishedDto>(itemPublishedMessage);
                try
                {
                    var item = _mapper.Map<Item>(itemPublishedDto);
                    if (!repo.ExternalItemExists(item.ExternalId))
                    {
                        repo.Create(item);
                        _logger.LogInformation("--> Item has been added!");
                    }
                    else
                    {
                        _logger.LogInformation("--> Item already exists");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"--> Could not add item to db. Error: {ex.Message}", ex);
                }
            }
        }
    }
    enum EventType
    {
        ItemPublished,
        Undetermined
    }
}
*/