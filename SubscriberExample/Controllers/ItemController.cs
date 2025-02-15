﻿using System.Threading.Tasks;
using InventoryService.DataAccess;
using InventoryService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ILogger _logger;

        public ItemController(ILoggerFactory logger, IItemClientProvider itemData)
        {
            _logger = logger.CreateLogger("InventoryServiceItemLogger");
        }

        /// <summary>
        /// Tests connection with the OrderService
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("testconnection/")]
        public Task<ItemReadDto> TestInBoundConnection([FromBody] ItemReadDto item)
        {
            _logger.LogInformation($"--> Inbound POST # ItemController inside InventoryService, Successfully grabbed {item.Id}");
            return Task.FromResult(item);
        }
    }
}
