using System;
#nullable enable
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    public delegate int CommandHandler(string prompt, string? cmd);
    public interface ICommandInteractive
    {
        [MobileSuitIgnore]
        void SetCommandHandler(CommandHandler commandHandler);
    }
}
