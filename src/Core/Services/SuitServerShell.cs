using System;
using Microsoft.Extensions.DependencyInjection;

namespace PlasticMetal.MobileSuit.Core.Services
{
    /// <summary>
    /// A SuitShell over SuitServer
    /// </summary>
    public class SuitServerShell : SuitObjectShell
    {
        internal SuitServerShell FromServer(Type serverType)
        {
            if (serverType.GetInterface(nameof(ISuitServer)) is null) throw new ArgumentOutOfRangeException(nameof(serverType));
            return new(serverType, s => s.ServiceProvider.GetRequiredService<ISuitServer>(), "SuitServer");
        }
        private SuitServerShell(Type type, InstanceFactory factory, string info) : base(type, factory, info)
        {
        }
    }
}