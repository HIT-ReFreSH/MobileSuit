using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Logging;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     A host of Mobile Suit, which may run commands.
    /// </summary>
    public interface IMobileSuitHost : IHost
    {
    }

}