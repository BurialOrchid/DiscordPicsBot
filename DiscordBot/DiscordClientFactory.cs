using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public class DiscordClientFactory
    {
        public static DiscordSocketClient CreateDiscordSocketClient() => new DiscordSocketClient(CreateDiscordSocketClientConfig());
        private static DiscordSocketConfig CreateDiscordSocketClientConfig() => new DiscordSocketConfig { AlwaysDownloadUsers = true, MessageCacheSize = 100 };
    }
}
