using Buckshout.Controllers;
using BuckshoutApp;
using BuckshoutApp.Context;
using StackExchange.Redis;
using System.Net.NetworkInformation;

namespace Buckshout
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            string redisConn = builder.Configuration.GetConnectionString("Redis");
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connection = redisConn;
                options.Configuration = connection;
            });
            //Чистим полностью редис перед запуском
            FlushRedis(redisConn!);

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

            ConfigureHost(app);

            app.MapHub<RoomHub>("/room");

            app.UseCors();

            app.Run();
        }
        public static void ConfigureHost(WebApplication app)
        {
            var ipAddressEthernet = NetworkUtils.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            var ipAddressWireless = NetworkUtils.GetLocalIPv4(NetworkInterfaceType.Wireless80211);

            app.Urls.Add($"http://localhost:5000");
            app.Urls.Add($"https://localhost:5001");
            if (ipAddressEthernet != null)
            {
                app.Urls.Add($"http://{ipAddressEthernet}:5000");
                app.Urls.Add($"https://{ipAddressEthernet}:5001");
            }
            if (ipAddressWireless != null)
            {

                app.Urls.Add($"http://{ipAddressWireless}:5000");
                app.Urls.Add($"https://{ipAddressWireless}:5001");
            }
        }
        private async static void FlushRedis(string redicConn)
        {

            ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync(redicConn);
            IDatabase db = redis.GetDatabase();

            await db.ExecuteAsync("FLUSHALL");
            Console.WriteLine("All Redis databases flushed.");
        }
    }
}