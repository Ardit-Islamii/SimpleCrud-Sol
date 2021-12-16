using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubscriberExample.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SubscriberExample.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IItemData _itemData;

        public ItemController(ILoggerFactory logger, IItemData itemData)
        {
            _logger = logger.CreateLogger("SubExampleItemLogger");
            _itemData = itemData;
        }
        [HttpPost("testconnection/")]
        public ActionResult TestInBoundConnection()
        {
            _logger.LogInformation("--> Inbound POST # ItemController inside SubExample");
            return Ok("Inbound test ok from ItemController inside subex.");
        }
    }
}
