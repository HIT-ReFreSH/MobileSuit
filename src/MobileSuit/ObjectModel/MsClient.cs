using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    public abstract class MsClient : IInfoProvider, IIoInteractive, ICommandInteractive
    {
        protected CommandHandler RunCommand { get; private set; }
        protected IoServer Io { get; private set; }

        [MsIgnorable]
        public void SetCommandHandler(CommandHandler commandHandler)
        {
            RunCommand = commandHandler;
        }

        public string Text { get; protected set; }

        [MsIgnorable]
        public void SetIo(IoServer io)
        {
            Io = io;
        }
    }
}