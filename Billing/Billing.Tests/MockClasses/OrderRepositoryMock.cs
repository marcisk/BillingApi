using Billing.Api.Data;
using Billing.Api.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Billing.Tests.MockClasses
{
    class OrderRepositoryMock : IOrderRepository
    {
        private Order[] _orders = new Order[]
        {
            new Order { Id = 1, Amount = 100, OrderNumber = "1" },
            new Order { Id = 2, Amount = 50, OrderNumber = "2" }
        };
        List<OrderBilling> _orderBillings = new List<OrderBilling>();

        /// <summary>
        /// Just simple mock method not to throw excception and simupate saving
        /// </summary>
        /// <param name="orderBilling"></param>
        /// <returns></returns>
        public int CreateOrderBilling(OrderBilling orderBilling)
        {
            orderBilling.Id = _orderBillings.Count + 1;
            _orderBillings.Add(orderBilling);

            return orderBilling.Id;
        }

        public Order GetOrder(string orderNumber)
        {
            return _orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
        }
    }
}
