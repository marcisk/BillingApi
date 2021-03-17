namespace Billing.Api.Data.Repository
{
    public interface IOrderRepository
    {
        int CreateOrderBilling(OrderBilling orderBilling);

        Order GetOrder(string orderNumber);
    }
}
