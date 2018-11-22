using System;
using DiscordBot.Common;

namespace DiscordBot
{
    [Serializable]
    class BotConfigurations : IBotConfigurations
    {
        public string Token => throw new NotImplementedException();

        public string HiPeople => throw new NotImplementedException();

        public ILoger Loger => throw new NotImplementedException();

        public ICommandManager CommandManager => throw new NotImplementedException();
    }
}
