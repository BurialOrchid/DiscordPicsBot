using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CommandHandlers
{
    public class PingHandler : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task HandleCommand(int? number = null, string thing = null)
        {       
            await ReplyAsync($"pong + {number} = {thing}");
        }
    }
}
