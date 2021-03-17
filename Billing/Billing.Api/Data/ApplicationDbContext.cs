using Microsoft.EntityFrameworkCore;

namespace Billing.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBilling> OrderBillings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
