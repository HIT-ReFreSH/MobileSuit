using System;
#nullable enable
using System.Collections.Generic;
using System.Text;

namespace MobileSuit.ObjectModel
{
    public delegate int CommandHandler(string prompt, string? cmd);
    public interface ICommandInteractive
    {
        void SetCommandHandler(CommandHandler commandHandler);
    }
}
