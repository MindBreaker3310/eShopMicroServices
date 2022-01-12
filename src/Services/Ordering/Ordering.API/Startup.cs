using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);

            // MassTransit-RabbitMQ Configuration
            services.AddMassTransit(config => {

                config.AddConsumer<BasketCheckoutConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();

            // General Configuration
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<BasketCheckoutConsumer>();
            ////可以看到AddMassTransit需要輸入Action type的參數，就是委託把不確定的動作，交由呼叫端來寫。
            //services.AddMassTransit(config =>
            //{
            //    config.AddConsumer<BasketCheckoutConsumer>();
            //    config.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfig) =>
            //    {
            //        //使用高級列隊協議amqp://帳號:密碼@網址:port
            //        rabbitMqBusFactoryConfig.Host(Configuration["EventBusSettings:HostAddress"]);

            //        rabbitMqBusFactoryConfig.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, endpointConfig =>
            //        {
            //            endpointConfig.ConfigureConsumer<BasketCheckoutConsumer>(busRegistrationContext);
            //        });
            //    });
            //});
            //services.AddMassTransitHostedService();

            //services.AddAutoMapper(typeof(Startup));
            //services.AddScoped<BasketCheckoutConsumer>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
