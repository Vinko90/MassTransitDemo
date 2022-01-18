using System;
using System.Timers;
using MTCore.Models;

namespace MTBlazorApp.Pages
{
	/// <summary>
    /// Code behind for Index Page
    /// </summary>
	public partial class Index
	{
		/// <summary>
        /// The current generated Order
        /// </summary>
        public Order CurrentOrder { get; set; }

		/// <summary>
        /// The total amount of orders processed
        /// </summary>
		public int TotalProcessedOrders { get; set; }

		/// <summary>
        /// Timer instance needed for automatically send messages to the REST API
        /// </summary>
		private readonly System.Timers.Timer autoTimer;

		/// <summary>
        /// Default Constructor
        /// </summary>
        public Index()
		{
			CurrentOrder = new Order();
			TotalProcessedOrders = 0;

            //Create a timer with 1 second interval
            autoTimer = new System.Timers.Timer(1000);
			autoTimer.Elapsed += OnTimedEvent;
			autoTimer.AutoReset = true;
			autoTimer.Enabled = false;
		}

		/// <summary>
        /// Generate a new GUID and send message to API
        /// </summary>
		private void SendMessage()
        {
			Console.WriteLine("Generating and processing new order!");

			CurrentOrder = new Order
			{
				OrderId = Guid.NewGuid().ToString(),
				OrderTime = DateTime.Now
			};

			TotalProcessedOrders++;

			StateHasChanged();
        }

		/// <summary>
        /// Toggle On/Off the Timer
        /// </summary>
		private void ToggleContinuous()
        {
			autoTimer.Enabled = !autoTimer.Enabled;
        }

		/// <summary>
        /// Event fired on timer tick
        /// </summary>
        /// <param name="source">Timer source object</param>
        /// <param name="e">Event args</param>
		private void OnTimedEvent(object? source, ElapsedEventArgs e)
		{
			SendMessage();
		}
	}
}

