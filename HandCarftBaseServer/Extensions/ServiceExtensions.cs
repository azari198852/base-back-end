using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Logger;
using Logger.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;


namespace HandCarftBaseServer.Extensions
{
    public static class ServiceExtensions
    {




        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }


        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILogHandler, LogHandler>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<BaseContext>(o => o.UseSqlServer(connectionString));

            var logconnectionString = config["ConnectionStrings:LogConnection"];
            services.AddDbContext<Log_DBContext>(o => o.UseSqlServer(logconnectionString));
        }


        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<ISyncDataBaseService,SyncDataBaseService>();
        }


    }
}
