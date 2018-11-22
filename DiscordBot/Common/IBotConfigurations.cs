namespace DiscordBot.Common
{
    public interface IBotConfigurations
    {
        string Token { get; }
        string HiPeople { get; }
        ILoger Loger { get; }
        ICommandManager CommandManager{get;}
    }
}
