using Discord;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public static class LogConfigurationService
    {
        public static Logger BuildLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();
        }

        public static Task ConfigureDiscordLogging(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    Log.Fatal(msg.Message);
                    Log.Fatal(msg.Exception.Message);
                    Log.Fatal(msg.Exception.InnerException.Message);
                    break;
                case LogSeverity.Error:
                    Log.Error(msg.Message);
                    break;
                case LogSeverity.Warning:
                    Log.Warning(msg.Message);
                    break;
                case LogSeverity.Info:
                    Log.Information(msg.Message);
                    break;
                case LogSeverity.Verbose:
                    Log.Verbose(msg.Message);
                    break;
                case LogSeverity.Debug:
                    Log.Debug(msg.Message);
                    break;
                default:
                    Log.Information(msg.Message);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
