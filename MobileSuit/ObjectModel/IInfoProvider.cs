using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    public interface IInfoProvider
    {
        [MobileSuitIgnore]
        string Prompt { get; }
    }
}
