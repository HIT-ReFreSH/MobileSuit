using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    public interface IIoInteractive
    {
        [MsIgnorable]
        void SetIo(IoServer io);
    }
}
