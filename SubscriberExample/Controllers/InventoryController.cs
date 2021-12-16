using Microsoft.AspNetCore.Mvc;
using SubscriberExample.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SubscriberExample.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        // GET api/c/<InventoryController>/guid-id.
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetInventoryFromItemId(Guid itemId)
        {
            var inventoryItem =  await _inventoryService.FindByItemID(itemId);
            if(inventoryItem != null)
            {
                return Ok(inventoryItem); 
            }
            return NotFound();
        }
    }
}
