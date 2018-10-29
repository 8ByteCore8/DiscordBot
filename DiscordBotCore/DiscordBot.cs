using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DiscordBotCore.Common;

namespace DiscordBotCore
{
    public class DiscordBot
    {
        private DiscordClient DiscordClient { get; }
        private ILoger Loger { get; }

        public DiscordBot(string token, ILoger loger)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            Loger = loger;

            DiscordClient discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            });

            discord.MessageCreated += Discord_MessageCreated;

            discord.ConnectAsync().Wait();

            if (Loger != null)
                Loger.Log($"Бот успешно подключён.");
        }

        ~DiscordBot()
        {
            DiscordClient.DisconnectAsync();
            DiscordClient.Dispose();

            if (Loger != null)
                Loger.Dispose();
        }

        private Task Discord_MessageCreated(MessageCreateEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
