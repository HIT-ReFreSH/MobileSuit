using System;
using Microsoft.Extensions.DependencyInjection;

namespace PlasticMetal.MobileSuit.Core.Services;

/// <summary>
///     A SuitShell over SuitServer
/// </summary>
public class SuitHostShell : SuitObjectShell
{
    private SuitHostShell(Type type, InstanceFactory factory, string info) : base(type, factory, info, "")
    {
    }

    /// <summary>
    ///     Create a Host shell from command server.
    /// </summary>
    /// <param name="serverType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static SuitHostShell FromCommandServer(Type serverType)
    {
        if (serverType.GetInterface(nameof(ISuitCommandServer)) is null)
            throw new ArgumentOutOfRangeException(nameof(serverType));
        return new SuitHostShell(serverType, s => s.ServiceProvider.GetRequiredService<ISuitCommandServer>(),
            "SuitServer");
    }
}