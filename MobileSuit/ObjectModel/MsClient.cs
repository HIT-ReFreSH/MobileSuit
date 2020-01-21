using System;
using System.Collections.Generic;
using System.Text;
using PlasticMetal.MobileSuit.IO;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    using Interfaces;
    using Attributes;
    public abstract class MsClient:IInfoProvider,IIoInteractive,ICommandInteractive
    {
        
        protected CommandHandler RunCommand { get; private set; }
        protected IoServer Io { get; private set; }
        public string Text { get; protected set; }
        [MsIgnorable]
        public void SetIo(IoServer io)
        {
            Io = io;
        }
        [MsIgnorable]
        public void SetCommandHandler(CommandHandler commandHandler)
        {
            RunCommand = commandHandler;
        }
    }
}
