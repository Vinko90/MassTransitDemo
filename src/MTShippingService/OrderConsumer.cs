using MassTransit;
using MTCore.Models;

namespace MTShippingService
{
    /// <summary>
    /// The OrderConsumer class.
    /// Takes care of consuming the Order messages from messaging queue.
    /// </summary>
    public class OrderConsumer : IConsumer<Order>
    {
        /// <summary>
        /// Logging instance
        /// </summary>
        readonly ILogger<OrderConsumer> _logger;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Consume method invoked when new Order is received
        /// </summary>
        /// <param name="context">Messaging context</param>
        /// <returns>Task result</returns>
        public Task Consume(ConsumeContext<Order> context)
        {
            _logger.LogInformation("Received Order ID: {id} from {Sender}...Shipping to customer!", 
                context.Message.OrderId, context.SourceAddress);

            return Task.CompletedTask;
        }
    }
}