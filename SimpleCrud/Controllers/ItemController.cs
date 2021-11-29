using Microsoft.AspNetCore.Mvc;
using SimpleCrud.Contracts.Services;
using SimpleCrud.Models;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _itemService.Get(id); 
            if(result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<ValuesController>/createitem/itemTemplate
        [HttpPost("createitem/")]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
           var result =  await _itemService.Create(item);
           if(result != null)
            {
                return CreatedAtAction(nameof(Get), new { id = result.Id}, result);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("updateitem/{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] Item item)
        {
            var existingItem = await _itemService.Get(id);
            if (existingItem != null)
            {
                var result = await _itemService.Update(item);
                return Accepted(result);
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("deleteitem/{id}")]
        public async Task<IActionResult> DeleteItem(Guid Id)
        {
            var result = await _itemService.Delete(Id);
            if (result)
            {
                return Accepted();
            }
            else
            {
                return NoContent();
            }
        }
    }
}
