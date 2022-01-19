using MTCore.Models;

namespace MTBlazorApp.Utils
{
    /// <summary>
    /// Interface definition for Order Service
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Submit an Order to the OrderService API
        /// </summary>
        /// <param name="order">The order Object</param>
        /// <returns>Http response status</returns>
        Task<HttpResponseMessage> SubmitOrder(Order order);
    }
}