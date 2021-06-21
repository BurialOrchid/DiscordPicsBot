using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.EventHandlers;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
    public partial class DiscordConfiguration
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _configuration;
        private readonly IMessageDeletedHandler _messageDeletedHandler;
        private readonly IMessageRecivedHandler _messageRecivedHandler;
        private readonly IMessageUpdatedHandler _messageUpdatedHandler;
        private readonly IServiceProvider _services;
        private readonly ICommandExecutedHandler _commandExecutedHandler;

        public DiscordConfiguration(CommandService commands,
                                    DiscordSocketClient client,
                                    IConfiguration configuration,
                                    IMessageDeletedHandler messageDeletedHandler,
                                    IMessageRecivedHandler messageRecivedHandler,
                                    IMessageUpdatedHandler messageUpdatedHandler,
                                    IServiceProvider services,
                                    ICommandExecutedHandler commandExecutedHandler)
        {
            _commands = commands;
            _client = client;
            _configuration = configuration;
            _messageDeletedHandler = messageDeletedHandler;
            _messageRecivedHandler = messageRecivedHandler;
            _messageUpdatedHandler = messageUpdatedHandler;
            _services = services;
            _commandExecutedHandler = commandExecutedHandler;
        }
        public async Task ConfigureClientAsync()
        {
            _client.Log += LogConfigurationService.ConfigureDiscordLogging;
            _client.MessageDeleted += _messageDeletedHandler.HandleEvent;
            _client.MessageUpdated += _messageUpdatedHandler.HandleEvent;
            _client.MessageReceived += _messageRecivedHandler.HandleEvent;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _commands.CommandExecuted += _commandExecutedHandler.HandleEvent;
            Log.Information($"Found {_commands.Commands.Count()} commands");
        }
        public async Task RunClientAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _configuration.GetValue<string>("DiscordToken"));
            await _client.StartAsync();
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        public async Task StopClient(DiscordSocketClient client)
        {
            await client.StopAsync();
            await client.LogoutAsync();
        }

        public async void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            await StopClient(_client);
        }
    }
}
