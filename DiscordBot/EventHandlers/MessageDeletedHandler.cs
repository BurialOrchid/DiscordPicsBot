using Discord;
using Discord.WebSocket;
using DiscordBot.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DiscordBot.EventHandlers
{
    public interface IMessageDeletedHandler
    {
        Task HandleEvent(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel);
    }

    public class MessageDeletedHandler : IMessageDeletedHandler
    {
        public async Task HandleEvent(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if(message != null)
                Log.Information($"Deleted: Ch: {message.Channel}, A: {message.Author}, M: {message.Content}");
        }

    }
}

