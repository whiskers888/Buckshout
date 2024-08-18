using Buckshout.Controllers;
using BuckshoutApp;
using BuckshoutApp.Context;
using System.Net.NetworkInformation;

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

            ConfigureHost(app);

            app.MapHub<RoomHub>("/room");

            app.UseCors();

            app.Run();
        }
        public static void ConfigureHost(WebApplication app)
        {
            var ipAddressWireless = NetworkUtils.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            var ipAddressEthernet = NetworkUtils.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            if (ipAddressWireless != null)
            {
                ConfigureIp(app, ipAddressWireless);
            }
            else if (ipAddressEthernet != null)
            {
                ConfigureIp(app, ipAddressEthernet);
            }
            else
            {
                Console.WriteLine("Server running only in local");
                app.Urls.Add("http://localhost:5000");
                app.Urls.Add("https://localhost:5001");
            }
        }

        public static void ConfigureIp(WebApplication app, string ipAddress)
        {
            app.Urls.Add($"http://localhost:5000");
            app.Urls.Add($"https://localhost:5001");
            app.Urls.Add($"http://{ipAddress}:5000");
            app.Urls.Add($"https://{ipAddress}:5001");
        }
    }
}