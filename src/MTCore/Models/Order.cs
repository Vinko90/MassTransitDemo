using System;

namespace MTCore.Models
{
    /// <summary>
    /// Model class used to send a new order to OrderService API
    /// </summary>
    public class Order
	{
        /// <summary>
        /// The Order ID
        /// </summary>
        public string OrderId { get; set; } = "0";

        /// <summary>
        /// The timestamp when the order was generated
        /// </summary>
        public DateTime OrderTime { get; set; }
	}
}