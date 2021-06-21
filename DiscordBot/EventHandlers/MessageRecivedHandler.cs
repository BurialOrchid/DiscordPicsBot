using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.EventHandlers
{
    public interface IMessageRecivedHandler
    {
        SocketCommandContext Context { get; set; }
        Task HandleEvent(SocketMessage arg);
    }

    public class MessageRecivedHandler : IMessageRecivedHandler
    {

        private readonly IDownloaderService _downloader;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public SocketCommandContext Context { get; set; }
        public MessageRecivedHandler(IDownloaderService downloader,
                                     DiscordSocketClient client,
                                     CommandService commands,
                                     IServiceProvider services)
        {
            _downloader = downloader;
            _client = client;
            _commands = commands;
            _services = services;
            _commands.Log += HandleCommandExceptionThrown;
        }

        public async Task HandleEvent(SocketMessage arg)
        {
            int argPos = 0;
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
                return;

            if (message.HasCharPrefix('!', ref argPos))
                await HandleCommandRecived(message, context, argPos);
            else
                await LogMessageRecived(message);
        }

        private async Task HandleCommandRecived(SocketUserMessage message, SocketCommandContext context, int argPos)
        => await _commands.ExecuteAsync(context, argPos, _services);

        public async Task HandleCommandExceptionThrown(LogMessage logMessage)
        {
            if (!(logMessage.Exception is CommandException cmdException))
                return;
            
            // We can tell the user that something unexpected has happened
            await cmdException.Context.Channel.SendMessageAsync("Something went catastrophically wrong!");

            // We can also log this incident
            Log.Error($"{cmdException.Context.User} failed to execute '{cmdException.Command.Name}' in {cmdException.Context.Channel}.");
            Log.Error(cmdException.ToString());
        }

        private async Task LogMessageRecived(SocketUserMessage message)
        {
            Log.Information($"Recived: Ch:{message.Channel}, A:{message.Author}, M:{message.Content}");
            if (message.Attachments.Any())
                await LogAttachmentsRecived(message.Attachments);
        }

        private async Task LogAttachmentsRecived(IEnumerable<Attachment> attachments)
        {
            Log.Information($"Attachments:");
            var attchmentList = attachments.ToList();
            attchmentList.ForEach(a =>
            {
                Log.Information($"Attachment: {attchmentList.IndexOf(a)} " +
                               $"Name:{a.Filename} " +
                               $"Size:{a.Size / 1024.0} " +
                               $"Spoiler:{a.IsSpoiler()}");
            });
        }
    }
}

