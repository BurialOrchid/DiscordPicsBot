using Discord;
using Discord.WebSocket;
using Serilog;
using System.Threading.Tasks;

namespace DiscordBot.EventHandlers
{
    public interface IMessageUpdatedHandler
    {
        public Task HandleEvent(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel);
    }

    public class MessageUpdatedHandler : IMessageUpdatedHandler
    {
        public async Task HandleEvent(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var message = await before.GetOrDownloadAsync();
            Log.Information($"Update: Ch: {message.Channel}, A: {message.Author}, M: {message.Content}");
            Log.Information($"To:     Ch: {after.Channel}, A: {after.Author}, M: {after.Content}");
        }
    }
}