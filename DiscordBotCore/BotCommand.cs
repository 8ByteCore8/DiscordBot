using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotCore
{
    enum CommandType { Discord,Console}
    sealed class BotCommand
    {
        public BotCommand(string text)
        {
            throw new NotImplementedException();
        }

        public CommandType Type { get; set; } = CommandType.Discord;

        public string Module { get; }

        public string Command { get; }

        public string[] Args { get; }

        public static BotCommand Parse(string text)
        {
            throw new NotImplementedException();
        }

        public static BotCommand TryParse(string text)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                return null;
            }
        }
    }
}
