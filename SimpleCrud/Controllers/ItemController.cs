using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using OrderService.ClientFactory;
using OrderService.Contracts.Services;
using OrderService.Dtos;
using OrderService.Extensions;
using OrderService.Options;
using OrderService.Providers;

namespace OrderService.Controllers
{   
    /// <summary>
    /// The controller for item crud.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger _logger;
        /*private readonly IMessageBusClient _messageBusClient;*/
        private readonly IMapper _mapper;
        /*private readonly IInventoryServiceDataClient _inventoryClient;*/
        private readonly IConfiguration _configuration;
        private readonly IClientFactory<IItemClientProvider> _itemClientFactory;
        private readonly IDistributedCache _cache;

        public ItemController(IItemService itemService, ILoggerFactory logger /*IMessageBusClient messageBusClient*/,
            IMapper mapper, IDistributedCache cache,
            /*IInventoryServiceDataClient inventoryClient,*/
            IConfiguration configuration,
            IClientFactory<IItemClientProvider> itemClientFactory)
        {
            _itemService = itemService;
            _logger = logger.CreateLogger("ItemControllerLogger");
            /*_messageBusClient = messageBusClient;*/
            _mapper = mapper;
            _cache = cache;
            /*_inventoryClient = inventoryClient;*/
            _configuration = configuration;
            _itemClientFactory = itemClientFactory;
        }

        /// <summary>
        /// Gets an item from the inventory table based on the id of the item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all the items from the inventory table.
        /// First it tries to get them from cache and if that fails it gets them from the database then caches them for 60 seconds.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new item inside the Items table.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        // POST api/<ValuesController>/createitem/itemTemplate
        [HttpPost("createitem/")]
        public async Task<ItemReadDto> CreateItem([FromBody] Item item)
        {
           ItemReadDto result =  await _itemService.Create(item);
           return result;
        }

        /// <summary>
        /// Updates an item inside the Items table.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes an item from Items table.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
