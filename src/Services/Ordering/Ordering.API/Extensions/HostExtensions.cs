using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        //Action<A,B>代表兩個輸入不回傳的封裝函數，就像(A,B)=>{ ... }方法
        public static IHost MigrateDatabase<TContext>(this IHost host,
                                                      Action<TContext, IServiceProvider> seeder,
                                                      int? retry = 10) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"開始遷移資料庫成{typeof(TContext).Name}的形狀");

                    //InvokeSeeder(seeder, context, services);
                    context.Database.Migrate();
                    seeder(context, services);

                    logger.LogInformation($"遷移資料庫成{typeof(TContext).Name}的形狀結束");
                }
                catch (Exception err)
                {
                    logger.LogError(err, "寫入資料庫時發生錯誤");

                    if (retry > 0)
                    {
                        retry--;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, seeder, retry);
                    }
                }
            }
            return host;
        }

        //private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider>seeder,
        //                                 TContext context,
        //                                 IServiceProvider services) where TContext : DbContext
        //{
        //    context.Database.Migrate();
        //    seeder(context, services);
        //}
    }
}
