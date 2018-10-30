using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DiscordBotCore.Common;

namespace DiscordBotCore
{
    public class DiscordBot : IDisposable
    {
        private DiscordClient DiscordClient { get; }
        public ILoger Loger { get; set; }

        public DiscordBot(string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

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
            Dispose();
        }

        private Task Discord_MessageCreated(MessageCreateEventArgs e)
        {
            return Task.Factory.StartNew(() =>
            {
                BotCommand command;
                try
                {
                    command = new BotCommand(e.Message.Content.ToLower().Trim())
                    {
                        Type = CommandType.Discord
                    };
                }
                catch
                {
                    e.Message.RespondAsync($"Не удалось разобрать команду: {e.Message.Content}");
                    Loger.Log($"Не удалось разобрать команду: {e.Message.Content}");
                    return;
                }
            });
        }

        public void Dispose()
        {
            DiscordClient.DisconnectAsync();
            DiscordClient.Dispose();

            if (Loger != null)
                Loger.Dispose();
        }
    }
}
