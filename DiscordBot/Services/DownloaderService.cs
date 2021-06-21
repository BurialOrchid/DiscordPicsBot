using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public interface IDownloaderService
    {
        Task<bool> DownloadMultipleFilesAsync(IEnumerable<Attachment> attachments);
        Task<bool> DownloadSingleFileAsync(Attachment attachment);
    }

    public class DownloaderService : IDownloaderService
    {
        private readonly IConfiguration _configuration;
        //  private readonly DiscordSocketClient _client;

        public DownloaderService(IConfiguration configuration,
            DiscordSocketClient client)
        {
            _configuration = configuration;
            //  _client = client;
        }

        public async Task<bool> DownloadSingleFileAsync(Attachment attachment)
        {
            var filePath = $"{_configuration.GetValue<string>("DownloadPath")}{attachment.Filename}";
            Log.Information($"Downloading: {attachment.Filename}");
            using WebClient client = new WebClient();
           await client.DownloadFileTaskAsync(new Uri(attachment.ProxyUrl), filePath);

            if (File.Exists(filePath))
            {
                Log.Information($"Downloaded: {attachment.Filename}");
                return true;
            }
            Log.Error($"Failed to download: {attachment.Filename}");
            return false;
        }

        public async Task<bool> DownloadMultipleFilesAsync(IEnumerable<Attachment> attachments)
        {
            int failedDownloads = 0;
            foreach (var attachment in attachments)
            {
                if (! await DownloadSingleFileAsync(attachment))
                    failedDownloads++;
            }
            return failedDownloads > 0;
        }
    }
}
