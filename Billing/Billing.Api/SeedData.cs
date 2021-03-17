using Billing.Api.Data;
using System;
using System.Linq;

namespace Billing.Api
{
    public static class SeedData
    {
        public static void Seed(ApplicationDbContext applicationDbContext)
        {
            var userCount = applicationDbContext.Users.Count();
            if (userCount == 0)
            {
                var user = new User
                {
                    UserName = "test"
                };
                applicationDbContext.Users.Add(user);
            }

            var orderCount = applicationDbContext.Orders.Count();
            if (orderCount == 0)
            {
                var orders = new Order[]
                {
                    new Order { Amount = 100.0M, OrderNumber = Guid.NewGuid().ToString() },
                    new Order { Amount = 200.0M, OrderNumber = Guid.NewGuid().ToString() }
                };
                applicationDbContext.Orders.AddRange(orders);
            }

            applicationDbContext.SaveChanges();
        }
    }
}
