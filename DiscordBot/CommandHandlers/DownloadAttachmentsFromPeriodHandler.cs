using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CommandHandlers
{

    public class DownloadAttachmentsFromPeriodHandler : ModuleBase<SocketCommandContext>
    {
        [Command("downloadp")]
        public async Task HandleCommand(char? period = null, int? quantity = null)
        {

            if (period is null)
            {
                await ReplyAsync("Specify period");
                return;
            }

            if (quantity is null)
            {
                await ReplyAsync("Specify quantity");
                return;
            }

            IMessage lastMessage = Context.Message;
            int messageNumber = 1;
            do
            {
                lastMessage = Context.Channel.GetMessagesAsync(lastMessage, Direction.Before, (int)1)
                                                                     .FlattenAsync().Result.FirstOrDefault();
                
                if (lastMessage != null && !lastMessage.Author.IsBot)
                {
                    //Log.Information($"{lastMessage.Content}");
                    messageNumber++;
                }
            }
            while (lastMessage != null);
            Log.Information($"This channel contains {messageNumber} messages");
        }
    }
}
