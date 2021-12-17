﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Models;
using OrderService.AsyncDataServices;
using OrderService.Contracts.Services;
using OrderService.Dtos;
using OrderService.Extensions;
using OrderService.SyncDataServices.Http;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger _logger;
        /*private readonly IMessageBusClient _messageBusClient;*/
        private readonly IMapper _mapper;
        private readonly IInventoryServiceDataClient _subscriberClient;
        private readonly IDistributedCache _cache;

        public ItemController(IItemService itemService, ILoggerFactory logger /*IMessageBusClient messageBusClient*/,
            IMapper mapper, IDistributedCache cache,
            IInventoryServiceDataClient subscriberClient)
        {
            _itemService = itemService;
            _logger = logger.CreateLogger("ItemControllerLogger");
            /*_messageBusClient = messageBusClient;*/
            _mapper = mapper;
            _cache = cache;
            _subscriberClient = subscriberClient;
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Item result = await _itemService.Get(id); 
            if(result != null)
            {
                _logger.LogInformation($"--> Successfully grabbed item: {result.Id}");
                return Ok(result);
            }
            _logger.LogInformation($"--> Could not find item with Id: {id}");
            return NotFound();
        }

        // GET api/<ValuesController>/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var recordKey = "ItemList";
            var itemsFromCache = await _cache.GetRecordAsync<List<Item>>(recordKey);

            if (itemsFromCache == null)
            {
                List<Item> itemsFromDatabase = await _itemService.Get();
                if (itemsFromDatabase != null)
                {
                    await _cache.SetRecordAsync(recordKey, itemsFromDatabase);
                    _logger.LogInformation($"--> Successfully grabbed items from database: {itemsFromDatabase}");
                    return Ok(itemsFromDatabase);
                }
                _logger.LogInformation("--> Could not find any items...");
                return NoContent();
            }
            _logger.LogInformation($"--> Successfully grabbed items from cache: {itemsFromCache}");
            return Ok(itemsFromCache);
        }

        // POST api/<ValuesController>/createitem/itemTemplate
        [HttpPost("createitem/")]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
           Item result =  await _itemService.Create(item);
           if(result != null)
           {
                _logger.LogInformation($"--> Successfully created item: {result}");
                var itemReadDto = _mapper.Map<ItemReadDto>(result);
                itemReadDto.Event = "Item_Published";

                //Send message sync using httpclient
                try
                {
                    await _subscriberClient.SendItemToSubExample(itemReadDto);
                }catch(Exception ex)
                {
                    _logger.LogInformation($"--> Could not send synchronously: {ex.Message}");
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
                return CreatedAtAction(nameof(Get), new { id = result.Id}, result);
           }
           _logger.LogInformation("--> Could not create item");
           return BadRequest();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("updateitem/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] Item item)
        {
            Item result = await _itemService.Update(item);
            if(result != null)
            {
                _logger.LogInformation($"--> Successfully updated item: {result.Id}");
                return Accepted(result);
            }
            _logger.LogWarning($"--> Could not find item with Id: {id}");
            return NotFound();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("deleteitem/{id}")]
        public async Task<IActionResult> DeleteItem(Guid Id)
        {
            bool result = await _itemService.Delete(Id);
            if (result)
            {
                _logger.LogInformation($"--> Successfully deleted item: {Id}");
                return Accepted();
            }
            _logger.LogWarning($"--> Could not find item with Id: {Id}");
            return NoContent();
        }
    }
}
