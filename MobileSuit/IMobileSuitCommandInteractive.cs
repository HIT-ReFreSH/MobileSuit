using System;
#nullable enable
using System.Collections.Generic;
using System.Text;

namespace MobileSuit
{
    public delegate int CommandHandler(string prompt, string? cmd);
    public interface IMobileSuitCommandInteractive
    {
        void SetCommandHandler(CommandHandler commandHandler);
    }
}
