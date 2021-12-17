using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InventoryService.Contracts.Services;
using InventoryService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;
        private readonly IMapper _mapper;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger, IMapper mapper)
        {
            _inventoryService = inventoryService;
            _logger = logger;
            _mapper = mapper;
        }

        // GET api/c/<InventoryController>/guid-id.
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetInventoryByItemId(Guid itemId, CancellationToken cancellationToken)
        {
            try
            {
                //Delaying task purposefully to test the cancellation token.
                await Task.Delay(4000);
                var inventoryItem = await _inventoryService.FindByItemID(itemId);
                if (inventoryItem != null)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("Request to grab inventory item cancelled.");
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    var inventoryReadDto = _mapper.Map<InventoryReadDto>(inventoryItem);
                    _logger.LogInformation(
                        $"--> Successfully grabbed the inventory of the item with id:{inventoryReadDto.ItemId}");
                    return Ok(inventoryReadDto);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
                return BadRequest();
            }
        }
    }
}
