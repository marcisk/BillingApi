using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Api.Data
{
    public class OrderBilling
    {
        [Key]
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public decimal PayableAmount { get; set; }
        public int PaymentGateway { get; set; }
        public string Description { get; set; }
    }
}
