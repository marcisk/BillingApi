using System.Linq;

namespace Billing.Api.Data.Repository.Impl
{
    public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int CreateOrderBilling(OrderBilling orderBilling)
        {
            _applicationDbContext.OrderBillings.Add(orderBilling);
            _applicationDbContext.SaveChanges();

            return orderBilling.Id;
        }

        public Order GetOrder(string orderNumber)
        {
            return _applicationDbContext.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
        }
    }
}
