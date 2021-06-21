using Discord;
using Discord.Commands;
using Serilog;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public interface ICommandExecutedHandler
    {
        Task HandleEvent(Optional<CommandInfo> info, ICommandContext context, IResult result);
    }

    public class CommandExecutedHandler : ICommandExecutedHandler
    {
        public async Task HandleEvent(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!string.IsNullOrEmpty(result?.ErrorReason))
                await context.Channel.SendMessageAsync(result.ErrorReason);

            var commandName = command.IsSpecified ? command.Value.Name : "A command";
            Log.Information($"CommandExecution: {commandName} was executed at {DateTime.UtcNow}.");
        }
    }
}