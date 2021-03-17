using System.ComponentModel.DataAnnotations;

namespace Billing.Api.Data
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
