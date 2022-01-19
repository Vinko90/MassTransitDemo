using System.Text;
using System.Text.Json;
using MTCore.Models;

namespace MTBlazorApp.Utils
{
    /// <summary>
    /// The Order Service Class.
    /// Used to submit orders to the OrderService API.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient instance</param>
        public OrderService(HttpClient httpClient)
        {
            client = httpClient;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SubmitOrder(Order order)
        {
            var json = JsonSerializer.Serialize(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync("api/order", data);
        }
    }
}