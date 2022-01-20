using OrderService.Contracts.Repositories;
using OrderService.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using OrderService.ClientFactory;
using OrderService.Dtos;
using OrderService.Options;
using OrderService.Providers;

namespace OrderService.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IClientFactory<IItemClientProvider> _itemClientFactory;
        private readonly IDistributedCache _cache;

        public ItemService(ILogger<ItemService> logger,
            IMapper mapper,
            IDistributedCache cache,
            IConfiguration configuration,
            IClientFactory<IItemClientProvider> itemClientFactory,
            IItemRepository itemRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
            _configuration = configuration;
            _itemClientFactory = itemClientFactory;
            _itemRepository = itemRepository;
        }

        public async Task<ItemReadDto> Create(Item item)
        {
            var result = await _itemRepository.Create(item);

            if (result != null)
            {
                _logger.LogInformation($"--> Successfully created item: {result}");
                var itemReadDto = _mapper.Map<ItemReadDto>(result);

                //Send a message sync using HttpClient to InventoryService on itemController
                try
                {
                    var uri = _configuration.GetSection(ItemOptions.DefaultSection + ":Uri").Value;
                    var itemClient = await _itemClientFactory.CreateClient(uri);
                    await itemClient.TestInboundConnection(itemReadDto);
                    return itemReadDto;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"--> Could not send synchronously: {ex.Message}", ex);
                }
                //Send message async using RabbitMQ -  Commented out due to not being relevant anymore.
                /*try
                {
                    _messageBusClient.PublishNewItem(itemReadDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"--> Could not send message asynchronously. Error: {ex.Message}");
                }*/
            }
            _logger.LogInformation("--> Could not create item");
            return null;
        }

        public async Task<bool> Delete(Guid Id)
        {
            Item item = await _itemRepository.Get(Id);
            if (item != null)
            {
                return await _itemRepository.Delete(item);
            }
            return false;
        }

        public async Task<Item> Get(Guid Id)
        {
            return await _itemRepository.Get(Id);
        }

        public async Task<List<Item>> Get()
        {
            return await _itemRepository.Get();
        }

        public async Task<Item> Update(Item item)
        {
            var existingItem = await _itemRepository.Get(item.Id);
            if(existingItem != null)
            {
                return await _itemRepository.Update(item);
            }
            return null;
        }
    }
}
