using PlasticMetal.MobileSuit.ObjectModel.Attributes;

#nullable enable

namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    public delegate TraceBack CommandHandler(string prompt, string? cmd);

    public interface ICommandInteractive
    {
        [MsIgnorable]
        void SetCommandHandler(CommandHandler commandHandler);
    }
}