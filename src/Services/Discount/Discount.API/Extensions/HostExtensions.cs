using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host, int retry = 10)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<T>>();

                try
                {
                    var connection = new NpgsqlConnection(configuration["DatabaseSettings:ConnectionString"]);
                    connection.Open();

                    var command = new NpgsqlCommand { Connection = connection };

                    //初始化Coupon資料表
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                                Product VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(Product, Description, Amount) VALUES('IPhone X', 'Apple學生專案', 150)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(Product, Description, Amount) VALUES('Samsung 10', '三星折價券', 100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("遷移postgres資料庫完成");
                }
                catch (Exception err)
                {
                    logger.LogError(err, "寫入資料庫時發生錯誤");

                    if(retry > 0)
                    {
                        retry--;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<T>(host, retry);
                    }
                }
            }
            return host;
        }
    }
}
