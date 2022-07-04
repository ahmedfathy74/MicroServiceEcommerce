using BasketApii.Repositories;
using BasketApii.Repositories.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApii
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


            //add redis connection

            services.AddStackExchangeRedisCache(op =>
            {
                op.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });


            //addd inject service
            services.AddScoped<IBasketRepository, BasketRepository>();

            // add auto maper

            services.AddAutoMapper(typeof(Startup));
            //add rabbit mq connection 
            //services.AddSingleton<IRabbitMQConnection>(sp =>
            //{
            //    var factory = new ConnectionFactory()
            //    {
            //        HostName = Configuration["EventBus:HostName"]
            //    };

            //    if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
            //    {
            //        factory.UserName = Configuration["EventBus:UserName"];
            //    }

            //    if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
            //    {
            //        factory.Password = Configuration["EventBus:Password"];
            //    }

            //    return new RabbitMQConnection(factory);
            //});

            //services.AddSingleton<EventBusRabbitMQProducer>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketApii", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketApii v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
