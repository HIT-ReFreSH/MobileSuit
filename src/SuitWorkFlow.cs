// /*
//  * Author: Ferdinand Sukhoi
//  * Email: ${User.Email}
//  * Date: 04 18, 2024
//  *
//  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Middleware;
using HitRefresh.MobileSuit.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Specifies the middleware collection of MobileSuit
/// </summary>
internal class SuitMiddlewareCollection : List<ISuitMiddleware>, ISuitMiddlewareCollection
{
    /// <inheritdoc />
    public SuitMiddlewareCollection(IEnumerable<ISuitMiddleware> enumerable) : base(enumerable) { }

    /// <inheritdoc />
    public SuitRequestDelegate BuildSuitRequestDelegate()
    {
        var requestStack = new Stack<SuitRequestDelegate>();
        requestStack.Push(_ => Task.CompletedTask);


        foreach (var middleware in (this as IEnumerable<ISuitMiddleware>).Reverse())
        {
            var next = requestStack.Peek();
            requestStack.Push(async c => await middleware.InvokeAsync(c, next));
        }

        var standardPipeline= requestStack.Peek();
        return async context=>
        {
            try
            {
                await standardPipeline(context);
            }
            catch (Exception ex)
            {
                context.Exception = ex;
                context.Status = RequestStatus.Faulted;
                await context.ServiceProvider.GetRequiredService<ISuitExceptionHandler>().InvokeAsync(context);
            }
        };
    }
}

/// <summary>
///     Describes the work flow of mobile suit.
/// </summary>
public interface ISuitWorkFlow
{
    /// <summary>
    ///     Get all middlewares configured
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
    IServiceCollection Build(IServiceCollection serviceProvider);
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
    public IServiceCollection Build(IServiceCollection serviceProvider)
    {
        if (_middlewares.Count == 0)
            UsePrompt()
               .UseIOInput()
               .UseRequestParsing()
               .UseHostShell()
               .UseAppShell()
               .UseFinalize();
        foreach (var middleware in _middlewares) serviceProvider.AddSingleton(middleware);

        serviceProvider.AddSingleton<ISuitMiddlewareCollection>
        (
            provider => new SuitMiddlewareCollection
            (
                _middlewares
                   .Select
                    (
                        t => provider.GetRequiredService(t) as ISuitMiddleware
                          ?? throw new ArgumentOutOfRangeException(t.FullName)
                    )
            )
        );
        return serviceProvider;
    }
}