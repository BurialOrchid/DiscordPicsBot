using Discord.Commands;
using DiscordBot.CommandHandlers;
using DiscordBot.EventHandlers;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordBot
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void BuildServiceProvider(IConfiguration configuration) 
        {
            ServiceProvider = ConfigureServices(configuration).BuildServiceProvider();
        }

        private static IServiceCollection ConfigureServices(IConfiguration configuration)
        {
            return new ServiceCollection()
            .AddSingleton(configuration)
            .AddSingleton(DiscordClientFactory.CreateDiscordSocketClient())
            .AddSingleton<DiscordConfiguration, DiscordConfiguration>()
            .AddSingleton(new CommandService())
            //Services
            .AddSingleton<IDownloaderService, DownloaderService>()

            //EventHandlers
            .AddTransient<IMessageRecivedHandler, MessageRecivedHandler>()
            .AddTransient<IMessageUpdatedHandler, MessageUpdatedHandler>()
            .AddTransient<IMessageDeletedHandler, MessageDeletedHandler>()
            .AddTransient<ICommandExecutedHandler, CommandExecutedHandler>();
            
        }
    }
}
