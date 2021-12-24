using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    //public class OrderContextSeed
    //{
    //    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    //    {
    //        //如果Orders資料表沒有資料的話
    //        if (orderContext.Orders.Any())
    //        {
    //            orderContext.AddRange(GetDefaultOrders());
    //            await orderContext.SaveChangesAsync();
    //            logger.LogInformation("寫入預設訂單", typeof(OrderContext).Name);
    //        }
    //    }

    //    private static IEnumerable<Order> GetDefaultOrders()
    //    {
    //        return new List<Order>
    //        {
    //            new Order()
    //            {
    //                UserName = "DefaultUser",
    //                FirstName = "Defalut",
    //                LastName = "User",
    //                EmailAddress = "test@gmail.com",
    //                AddressLine = "Taipei 101",
    //                Country = "Taiwan",
    //                TotalPrice = 999
    //            }
    //        };
    //    }
    //}
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "swn", FirstName = "Mehmet", LastName = "Ozkaya", EmailAddress = "ezozkme@gmail.com", AddressLine = "Bahcelievler", Country = "Turkey", TotalPrice = 350 }
            };
        }
    }
}
