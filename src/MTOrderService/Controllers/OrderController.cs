using Microsoft.AspNetCore.Mvc;
using MTCore.Models;

namespace MTOrderService.Controllers
{
    /// <summary>
    /// Order API Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger">Logger interface</param>
        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Submit new Order into the system
        /// </summary>
        /// <param name="order">Order object</param>
        /// <returns>Action Result</returns>
        /// <response code="200">Order received and forwared for shipping</response>
        /// <response code="400">The order is null or the ID is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SubmitOrder(Order order)
        {
            if (order != null && order.OrderId != "0")
            {
                _logger.LogInformation($"Received new order: {order.OrderId}");

                return Ok();
            }

            return BadRequest();
        }
    }
}

