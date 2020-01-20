using System;
using System.Collections.Generic;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.IO
{
    public interface IIoInteractive
    {
        [MobileSuitIgnore]
        void SetIo(IoInterface io);
    }
}
