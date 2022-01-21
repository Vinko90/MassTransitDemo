using MassTransit;
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
        private readonly IBus _bus;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger">Logger interface</param>
        /// <param name="bus">The service bus</param>
        public OrderController(ILogger<OrderController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
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
                _logger.LogInformation($"Received new order: {order.OrderId}, forwarding for shipping...");
                _logger.LogWarning("Warning message for testing logger!");
                await _bus.Publish(order);
                return Ok();
            }

            return BadRequest();
        }
    }
}

