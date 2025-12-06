using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Themes;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace HitRefresh.MobileSuit;

/// <summary>
/// Extension methods for SuitHostBuilder.
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
    /// Set the application name.
    /// </summary>
    /// <param name="builder">The builder</param>
    /// <param name="name">Application name</param>
    /// <returns>The builder</returns>
    public static SuitHostBuilder HasName(this SuitHostBuilder builder, string name)
    {
        builder.AppInfo.AppName = name;  // 修正：使用 AppName 而不是 Name
        return builder;
    }

    /// <summary>
    /// Use PowerLine prompt.
    /// </summary>
    public static SuitHostBuilder UsePowerLine(this SuitHostBuilder builder)
    {

        // builder.Services.AddSingleton<PromptFormatter>(PromptFormatters.PowerLinePromptFormatter);
        return builder;
    }

    /// <summary>
    /// Use 4-bit color IO.
    /// </summary>
    public static SuitHostBuilder Use4BitColorIO(this SuitHostBuilder builder)
    {
        builder.ConfigureIO(_ => { });
        return builder;
    }

/// <summary>
/// Map a client type.
/// </summary>
public static SuitHostBuilder MapClient<T>(this SuitHostBuilder builder)
{
    Console.WriteLine($"DEBUG: MapClient called for type {typeof(T).FullName}");

    try
    {
        // 使用 SuitObjectShell.FromType
        var suitShell = SuitObjectShell.FromType(typeof(T));
        Console.WriteLine($"DEBUG: SuitShell created. MemberCount: {suitShell.MemberCount}");

        builder.AddClient(suitShell);
        Console.WriteLine($"DEBUG: Client added to builder");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"DEBUG: Error in MapClient: {ex.Message}");
        Console.WriteLine($"DEBUG: StackTrace: {ex.StackTrace}");
    }

    return builder;
}

    /// <summary>
    /// Use Nord color theme.
    /// </summary>
    public static SuitHostBuilder UseNordTheme(this SuitHostBuilder builder)
    {
        builder.Services.AddSingleton<IColorSetting, NordTheme>();
        return builder;
    }

    /// <summary>
    /// Use Dracula color theme.
    /// </summary>
    public static SuitHostBuilder UseDraculaTheme(this SuitHostBuilder builder)
    {
        builder.Services.AddSingleton<IColorSetting, DraculaTheme>();
        return builder;
    }

    /// <summary>
    /// Use Solarized light theme.
    /// </summary>
    public static SuitHostBuilder UseSolarizedLightTheme(this SuitHostBuilder builder)
    {
        builder.Services.AddSingleton<IColorSetting, SolarizedLightTheme>();
        return builder;
    }

    /// <summary>
    /// Use Solarized dark theme.
    /// </summary>
    public static SuitHostBuilder UseSolarizedDarkTheme(this SuitHostBuilder builder)
    {
        builder.Services.AddSingleton<IColorSetting, SolarizedDarkTheme>();
        return builder;
    }

    /// <summary>
    /// Use Monokai theme.
    /// </summary>
    public static SuitHostBuilder UseMonokaiTheme(this SuitHostBuilder builder)
    {
        builder.Services.AddSingleton<IColorSetting, MonokaiTheme>();
        return builder;
    }

    /// <summary>
    /// Use custom color theme.
    /// </summary>
    /// <typeparam name="T">Theme type implementing IColorSetting</typeparam>
    public static SuitHostBuilder UseCustomTheme<T>(this SuitHostBuilder builder)
        where T : class, IColorSetting
    {
        builder.Services.AddSingleton<IColorSetting, T>();
        return builder;
    }
}