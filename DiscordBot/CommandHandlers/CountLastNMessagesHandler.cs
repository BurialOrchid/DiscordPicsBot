using Discord;
using Discord.Commands;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.CommandHandlers
{
    public class CountLastNMessagesHandler : ModuleBase<SocketCommandContext>
    {
        [Command("lastn")]
        public async Task HandleCommand(int numberOfMessages)
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, (int)numberOfMessages)
                                                                  .FlattenAsync();
            await ReplyAsync($"{Context.Channel} has {messages.Count()} messages");
            var botMessagesQuantity = messages.Count(m => m.Author.IsBot);
            var userMessagesQuantity = messages.Count(m => !m.Author.IsBot);

            await ReplyAsync($"In last {numberOfMessages} bot has written {botMessagesQuantity} messages");
            await ReplyAsync($"In last {numberOfMessages} users have written {userMessagesQuantity} messages");

        }
    }
}