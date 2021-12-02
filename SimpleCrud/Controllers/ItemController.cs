using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SimpleCrud.AsyncDataServices;
using SimpleCrud.Contracts.Services;
using SimpleCrud.Dtos;
using SimpleCrud.Extensions;
using SimpleCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger _logger;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ItemController(IItemService itemService, ILoggerFactory logger, IMessageBusClient messageBusClient,
            IMapper mapper, IDistributedCache cache)
        {
            _itemService = itemService;
            _logger = logger.CreateLogger("ItemControllerLogger");
            _messageBusClient = messageBusClient;
            _mapper = mapper;
            _cache = cache;
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _itemService.Get(id); 
            if(result != null)
            {
                _logger.LogInformation($"Successfully grabbed item: {result.Id}", result);
                return Ok(result);
            }
            else
            {
                _logger.LogInformation($"Could not find item with Id: {id}", id);
                return NotFound();
            }
        }

        // GET api/<ValuesController>/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Item> itemsFromCache = new List<Item>();
            var recordKey = "ItemList";
            itemsFromCache = await _cache.GetRecordAsync<List<Item>>(recordKey);

            if (itemsFromCache == null)
            {
                var itemsFromDatabase = await _itemService.Get();
                if(itemsFromDatabase != null)
                {
                    await _cache.SetRecordAsync(recordKey, itemsFromDatabase);
                    _logger.LogInformation($"Successfully grabbed items from database: {itemsFromDatabase}", itemsFromDatabase);
                    return Ok(itemsFromDatabase);
                }
                else
                {
                    _logger.LogInformation($"Could not find any items...");
                    return NoContent();
                }
            }
            else
            {
                _logger.LogInformation($"Successfully grabbed items from cache: {itemsFromCache}", itemsFromCache);
                return Ok(itemsFromCache);
            }
            
        }

        // POST api/<ValuesController>/createitem/itemTemplate
        [HttpPost("createitem/")]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
           var result =  await _itemService.Create(item);
           if(result != null)
            {
                _logger.LogInformation($"Successfully created item: {result}", result);
                var itemReadDto = _mapper.Map<ItemReadDto>(result);
                itemReadDto.Event = "Item_Published";

                //Send message async
                try
                {
                    _messageBusClient.PublishNewItem(itemReadDto);
                }catch(Exception ex)
                {
                    _logger.LogError($"--> Couldn't send message. Error: {ex.Message}", ex.Message);
                }

                return CreatedAtAction(nameof(Get), new { id = result.Id}, result);
            }
            else
            {
                _logger.LogInformation("Could not create item");
                return BadRequest();
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("updateitem/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] Item item)
        {
            var result = await _itemService.Update(item);
            if(result != null)
            {
                _logger.LogInformation($"Successfully updated item: {result.Id}", result.Id);
                return Accepted(result);
            }
            else
            {
                _logger.LogWarning($"Could not find item with Id: {id}", id);
                return NotFound();
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("deleteitem/{id}")]
        public async Task<IActionResult> DeleteItem(Guid Id)
        {
            var result = await _itemService.Delete(Id);
            if (result)
            {
                _logger.LogInformation($"Successfully deleted item: {Id}", Id);
                return Accepted();
            }
            else
            {
                _logger.LogWarning($"Could not find item with Id: {Id}", Id);
                return NoContent();
            }
        }
    }
}
