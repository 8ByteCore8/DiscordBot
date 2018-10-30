namespace DiscordBotCore.Common.Commands
{
    public interface ICommand
    {
        string[] Args { get; }
        string Command { get; }
        string Module { get; }
    }
}
