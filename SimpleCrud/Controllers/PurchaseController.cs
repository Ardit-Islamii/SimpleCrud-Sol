using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Nest;
using OrderService.ClientFactory;
using OrderService.Contracts.Services;
using OrderService.DataAccess;
using OrderService.Dtos;
using OrderService.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderService.Controllers
{
    /// <summary>
    /// The controller for creating purchases.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(IPurchaseService purchaseService, ILogger<PurchaseController> logger)
        {
            _purchaseService = purchaseService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a purchase based on itemId.
        /// First it gets the quantity available for that item and checks if its more than zero.
        /// If there are no available items, itll send a bad request.
        /// If there are available items it'll create a purchase and publish a message to the InventoryService on the purchaseConsumer class and decrement an item on the inventory there.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST api/<PurchaseController>
        [HttpPost("createpurchase/{id}")]
        public async Task<IActionResult> Post(Guid id)
        {
            var result = await _purchaseService.Create(id);

            return Ok(result);
        }
    }
}
