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
    /// Get all middlewares configured
    /// </summary>
    public IReadOnlyList<Type> Middlewares { get; }
    /// <summary>
    ///     Remove all existing middlewares
    /// </summary>
    public ISuitWorkFlow Reset();
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

    /// <summary>
    ///     Build workflow into middleware list.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    IReadOnlyList<ISuitMiddleware> Build(IServiceProvider serviceProvider);
}

/// <summary>
///     Default Implementation of ISuitWorkFlow
/// </summary>
public class SuitWorkFlow : ISuitWorkFlow
{
    private readonly List<Type> _middlewares = new();
    /// <inheritdoc />
    public IReadOnlyList<Type> Middlewares => _middlewares;

    /// <inheritdoc />
    public ISuitWorkFlow Reset()
    {
        _middlewares.Clear();
        return this;
    }

    /// <inheritdoc />
    public ISuitWorkFlow UseCustom(Type middlewareType)
    {
        if (middlewareType.GetInterface(nameof(ISuitMiddleware)) is not null)
            _middlewares.Add(middlewareType);
        else
            throw new ArgumentOutOfRangeException(nameof(middlewareType));
        return this;
    }

    /// <inheritdoc />
    public ISuitWorkFlow UsePrompt() { return this.UseCustom<PromptMiddleware>(); }

    /// <inheritdoc />
    public ISuitWorkFlow UseIOInput() { return this.UseCustom<IOInputMiddleware>(); }

    /// <inheritdoc />
    public ISuitWorkFlow UseRequestParsing() { return this.UseCustom<RequestParsingMiddleware>(); }

    /// <inheritdoc />
    public ISuitWorkFlow UseAppShell() { return this.UseCustom<AppShellMiddleware>(); }

    /// <inheritdoc />
    public ISuitWorkFlow UseHostShell() { return this.UseCustom<HostShellMiddleware>(); }

    /// <inheritdoc />
    public ISuitWorkFlow UseFinalize() { return this.UseCustom<FinalizeMiddleware>(); }

    /// <inheritdoc />
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