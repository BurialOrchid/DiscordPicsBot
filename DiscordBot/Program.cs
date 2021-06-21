using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.CommandHandlers;
using DiscordBot.EventHandlers;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public async static Task MainAsync(string[] args) 
        {
            IConfiguration configuration = BuildConfig();
            Log.Logger = LogConfigurationService.BuildLogger(configuration);

            Log.Information("Building configuration");

            ServiceProviderFactory.BuildServiceProvider(configuration);
            DiscordConfiguration svc = ActivatorUtilities.CreateInstance<DiscordConfiguration>(ServiceProviderFactory.ServiceProvider);

            Log.Information("App Starting");
            Console.CancelKeyPress += svc.Console_CancelKeyPress;

            await svc.ConfigureClientAsync();
            svc.RunClientAsync().GetAwaiter().GetResult();
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }


    }
}
