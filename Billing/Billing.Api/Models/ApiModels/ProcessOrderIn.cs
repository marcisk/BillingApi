namespace Billing.Api.Models.ApiModels
{
    public class ProcessOrderIn
    {
        public string OrderNumber { get; set; }
        public int UserId { get; set; }
        public decimal PayableAmount { get; set; }
        public int PaymentGateway { get; set; }
        public string Description { get; set; }
    }
}
