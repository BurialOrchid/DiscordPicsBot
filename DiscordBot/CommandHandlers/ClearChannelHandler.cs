using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CommandHandlers
{
    public class ClearChannelHandler : ModuleBase<SocketCommandContext>
    {
        [Command("clear")]
        public async Task HandleCommand()
        {
            IEnumerable<IMessage> messages = await Context.Channel
                                                              .GetMessagesAsync(Context.Message, Direction.Before, 100)
                                                              .FlattenAsync();
            foreach (var message in messages)
            {
                await message.DeleteAsync();
            }
        }
    }
}
