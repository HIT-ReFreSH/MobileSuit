using HitRefresh.MobileSuit.Core;
using Microsoft.Extensions.DependencyInjection;

namespace HitRefresh.MobileSuit.Themes;

/// <summary>
/// Extension methods for configuring themes in MobileSuit.
/// </summary>
public static class ThemeExtensions
{
    /// <summary>
    /// Use Nord color theme.
    /// </summary>
    public static IServiceCollection UseNordTheme(this IServiceCollection services)
    {
        return services.AddSingleton<IColorSetting, NordTheme>();
    }

    /// <summary>
    /// Use Dracula color theme.
    /// </summary>
    public static IServiceCollection UseDraculaTheme(this IServiceCollection services)
    {
        return services.AddSingleton<IColorSetting, DraculaTheme>();
    }

    /// <summary>
    /// Use Solarized light theme.
    /// </summary>
    public static IServiceCollection UseSolarizedLightTheme(this IServiceCollection services)
    {
        return services.AddSingleton<IColorSetting, SolarizedLightTheme>();
    }

    /// <summary>
    /// Use Solarized dark theme.
    /// </summary>
    public static IServiceCollection UseSolarizedDarkTheme(this IServiceCollection services)
    {
        return services.AddSingleton<IColorSetting, SolarizedDarkTheme>();
    }

    /// <summary>
    /// Use Monokai theme.
    /// </summary>
    public static IServiceCollection UseMonokaiTheme(this IServiceCollection services)
    {
        return services.AddSingleton<IColorSetting, MonokaiTheme>();
    }

    /// <summary>
    /// Use custom theme.
    /// </summary>
    public static IServiceCollection UseCustomTheme<T>(this IServiceCollection services)
        where T : class, IColorSetting
    {
        return services.AddSingleton<IColorSetting, T>();
    }
}