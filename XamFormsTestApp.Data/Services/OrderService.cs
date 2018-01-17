using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamFormsTestApp.Data.Model;
using System.Collections.ObjectModel;

namespace XamFormsTestApp.Data.Services
{
    public class OrderService : IOrderService
    {

        private readonly List<Order> _orders = new List<Order>();
        private readonly List<OrderDetails> _orderDetails = new List<OrderDetails>();

        public OrderService()
        {

            for (var i = 1; i <= 20; i++)
            {
                var ves = new Order {OrdId = i.ToString(), OrdName = "Order " + i};
                _orders.Add(ves);

                var det = new OrderDetails
                {
                    OrdId = i.ToString(),
                    OrdName = "Order " + i,
                    LastUpdated = DateTime.Now,
                    Speed = i,
                    Status = "On Order",
                    From = "Port" + i,
                    To = "New York",
                    Cargo = "Xamarin T-Shirts"
                };
                _orderDetails.Add(det);
            }
        }

        /// <summary>
        ///     Get the vessel position data
        /// </summary>
        /// <returns>
        ///     List of vessel objects
        /// </returns>
        public async Task<ObservableCollection<Order>> GetOrders()
        {

            var vess = from vessel in _orders
                       select vessel;
            return await Task.Run(() => new ObservableCollection<Order>(vess));
        }

        /// <summary>
        ///     Get the vessel position data
        /// </summary>
        /// <returns>
        ///     Vessel details object is returned
        /// </returns>
        public async Task<OrderDetails> GetOrderDetails(string vesid)
        {
            var vessdet = from d in _orderDetails
                          where d.OrdId == vesid
                          select d;
            return await Task.Run(() => vessdet.FirstOrDefault());
        }
    }
}