// /*
//  * Author: Ferdinand Sukhoi
//  * Email: ${User.Email}
//  * Date: 04 18, 2024
//  *
//  */

using System;
using System.Collections.Generic;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Describes the work flow of mobile suit.
/// </summary>
public interface ISuitWorkFlow
{
    /// <summary>
    ///     Add a custom middleware
    /// </summary>
    /// <param name="middlewareType"></param>
    public ISuitWorkFlow UseCustom(Type middlewareType);

    /// <summary>
    ///     Add suit prompt middleware
    /// </summary>
    public ISuitWorkFlow UsePrompt();

    /// <summary>
    ///     Add IOHub input middleware
    /// </summary>
    public ISuitWorkFlow UseIOInput();
    /// <summary>
    ///     Add
    /// </summary>
    /// <returns></returns>
    public ISuitWorkFlow UseRequestParsing();
    /// <summary>
    ///     Add AppShell middleware
    /// </summary>
    public ISuitWorkFlow UseAppShell();

    /// <summary>
    ///     Add HostShell middleware
    /// </summary>
    public ISuitWorkFlow UseHostShell();

    /// <summary>
    ///     Add Finalize middleware
    /// </summary>
    public ISuitWorkFlow UseFinalize();
}

internal class SuitWorkFlow : ISuitWorkFlow
{
    private readonly List<Type> _middlewares = new();

    public ISuitWorkFlow UseCustom(Type middlewareType)
    {
        if (middlewareType.GetInterface(nameof(ISuitMiddleware)) is not null)
            _middlewares.Add(middlewareType);
        else
            throw new ArgumentOutOfRangeException(nameof(middlewareType));
        return this;
    }

    public ISuitWorkFlow UsePrompt() { return this.UseCustom<PromptMiddleware>(); }

    public ISuitWorkFlow UseIOInput() { return this.UseCustom<IOInputMiddleware>(); }
    public ISuitWorkFlow UseRequestParsing() { return this.UseCustom<RequestParsingMiddleware>(); }
    public ISuitWorkFlow UseAppShell() { return this.UseCustom<AppShellMiddleware>(); }

    public ISuitWorkFlow UseHostShell() { return this.UseCustom<HostShellMiddleware>(); }

    public ISuitWorkFlow UseFinalize() { return this.UseCustom<FinalizeMiddleware>(); }

    public IReadOnlyList<ISuitMiddleware> Build(IServiceProvider serviceProvider)
    {
        if (_middlewares.Count == 0)
            UsePrompt()
               .UseIOInput()
               .UseRequestParsing()
               .UseHostShell()
               .UseAppShell()
               .UseFinalize();
        var r = new List<ISuitMiddleware>();
        foreach (var type in _middlewares)
            if (ActivatorUtilities.CreateInstance(serviceProvider, type) is ISuitMiddleware middleware)
                r.Add(middleware);
            else
                throw new ArgumentOutOfRangeException(type.FullName);
        return r;
    }
}