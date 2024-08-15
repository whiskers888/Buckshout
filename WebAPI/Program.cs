using Buckshout.Controllers;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace Buckshout
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });


            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed((host) => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddSignalR();
            builder.Services.AddSingleton<ApplicationContext>();
            var app = builder.Build();

            app.MapHub<RoomHub>("/room");

            app.UseCors();

            app.Run();
        }

    }
}