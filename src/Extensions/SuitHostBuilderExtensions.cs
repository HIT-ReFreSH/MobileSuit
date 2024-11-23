// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System;
using System.Text;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Extensions for MobileSuitHostBuilder
/// </summary>
public static class SuitHostBuilderExtensions
{
    /// <summary>
    ///     Add a custom middleware
    /// </summary>
    public static ISuitWorkFlow UseCustom<T>(this ISuitWorkFlow workFlow)
    where T : ISuitMiddleware
    {
        return workFlow.UseCustom(typeof(T));
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <typeparam name="T">PromptServer</typeparam>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder) where T : class
    {
        builder.Services.AddScoped<T>();
        builder.AddClient(SuitObjectShell.FromType(typeof(T)));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name">Set a name for the client</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder HasName(this SuitHostBuilder builder, string name)
    {
        builder.AppInfo.AppName = name;
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="instance"></param>
    /// <typeparam name="T">PromptServer</typeparam>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder, T instance) where T : class
    {
        builder.Services.AddSingleton(instance);
        builder.AddClient(SuitObjectShell.FromInstance(typeof(T), _ => instance));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name"></param>
    /// <typeparam name="T">PromptServer</typeparam>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder, string name) where T : class
    {
        builder.Services.AddScoped<T>();
        builder.AddClient(SuitObjectShell.FromType(typeof(T), name));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name"></param>
    /// <param name="instance"></param>
    /// <typeparam name="T">PromptServer</typeparam>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder, string name, T instance) where T : class
    {
        builder.Services.AddScoped<T>();
        builder.AddClient(SuitObjectShell.FromInstance(typeof(T), _ => instance, name));
        return builder;
    }

    /// <summary>
    ///     Use given PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <param name="name"></param>
    /// <param name="method"></param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder Map(this SuitHostBuilder builder, string name, Delegate method)
    {
        builder.AddClient(SuitMethodShell.FromDelegate(name, method));
        return builder;
    }

    /// <summary>
    ///     Use PowerLine PromptGenerator for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder UsePowerLine(this SuitHostBuilder builder)
    {
        Console.OutputEncoding = Encoding.UTF8;
        builder.Services.AddSingleton<PromptFormatter>(PromptFormatters.PowerLineFormatter);
        return builder;
    }

    /// <summary>
    ///     Use Plain text IO for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder UsePureTextIO(this SuitHostBuilder builder) { return builder.UseIO<PureTextIOHub>(); }

    /// <summary>
    ///     Use 4-bit color IO for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder Use4BitColorIO(this SuitHostBuilder builder) { return builder.UseIO<FourBitIOHub>(); }

    /// <summary>
    ///     Use 4-bit color IO for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder UseTrueColorIO(this SuitHostBuilder builder)
    {
        return builder.UseIO<TrueColorIOHub>();
    }

    /// <summary>
    ///     Add all middleware and the request delegate to the collection
    /// </summary>
    /// <param name="servicesCollection"></param>
    /// <param name="workFlow"></param>
    /// <returns></returns>
    public static IServiceCollection AddSuitMiddlewares
        (this IServiceCollection servicesCollection, ISuitWorkFlow workFlow)
    {
        return workFlow.Build(servicesCollection);
    }

    /// <summary>
    ///     Use certain IO for the Host
    /// </summary>
    /// <param name="builder">Builder for the host</param>
    /// <returns>Builder for the host</returns>
    public static SuitHostBuilder UseIO<T>(this SuitHostBuilder builder)
    where T : class, IIOHub
    {
        builder.Services.AddScoped<IIOHubYouShouldNeverUse, T>();
        return builder;
    }
    ///// <summary>
    /////     Run a mobile suit
    ///// </summary>
    ///// <param name="host"></param>
    ///// <returns></returns>
    //public static async Task RunAsync(this IMobileSuitHost host)
    //{
    //    var token = new CancellationTokenSource().Token;
    //    await host.StartAsync(token).ConfigureAwait(false);
    //    await host.StopAsync(token).ConfigureAwait(false);
    //    host.Dispose();
    //}

    /// <summary>
    ///     Run a mobile suit
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static void Run(this IMobileSuitHost host) { host.RunAsync().GetAwaiter().GetResult(); }
}