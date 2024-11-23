using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Providing Common APIs for MobileSuit
/// </summary>
public static class Suit
{
    /// <summary>
    ///     Quick start a mobilesuit on specific client class with 4bit IO and PowerLine
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    public static async void QuickStart4BitPowerLine<T>(string[] args)
    {
        await CreateBuilder(args)
             .HasName("demo")
             .UsePowerLine()
             .Use4BitColorIO()
             .MapClient<T>()
             .Build()
             .RunAsync();
    }

    /// <summary>
    ///     Quick start a mobilesuit on specific client class with default settings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    public static void QuickStart<T>(string[] args)
    {
        CreateBuilder(args)
           .HasName("demo")
           .UsePowerLine()
           .Use4BitColorIO()
           .MapClient<T>()
           .Build()
           .Run();
    }

    /// <summary>
    ///     Quick start a mobilesuit on specific client class with PowerLine
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    public static void QuickStartPowerLine<T>(string[] args)
    {
        CreateBuilder(args)
           .HasName("demo")
           .UsePowerLine()
           .MapClient<T>()
           .Build()
           .Run();
    }

    /// <summary>
    ///     Get a builder to create host
    /// </summary>
    /// <returns>The builder</returns>
    public static SuitHostBuilder CreateBuilder(string[]? args = null) { return new SuitHostBuilder(args); }

    /// <summary>
    ///     provides packaging for ContentArray
    /// </summary>
    /// <param name="contents">ContentArray</param>
    /// <returns>packaged ContentArray</returns>
    public static IEnumerable<PrintUnit> CreateContentArray(params (string, Color?)[] contents)
    {
        return SuitUtils.CreateContentArray(contents);
    }


    /// <summary>
    ///     provides packaging for ContentArray
    /// </summary>
    /// <param name="contents">ContentArray</param>
    /// <returns>packaged ContentArray</returns>
    public static IEnumerable<PrintUnit> CreateContentArray(params (string, Color?, Color?)[] contents)
    {
        return SuitUtils.CreateContentArray(contents);
    }

    /// <summary>
    ///     provides packaging for ContentArray
    /// </summary>
    /// <param name="contents">ContentArray</param>
    /// <returns>packaged ContentArray</returns>
    public static IEnumerable<PrintUnit> CreateContentArray(params (string, ConsoleColor?)[] contents)
    {
        return SuitUtils.CreateContentArray(contents);
    }


    /// <summary>
    ///     provides packaging for ContentArray
    /// </summary>
    /// <param name="contents">ContentArray</param>
    /// <returns>packaged ContentArray</returns>
    public static IEnumerable<PrintUnit> CreateContentArray(params (string, ConsoleColor?, ConsoleColor?)[] contents)
    {
        return SuitUtils.CreateContentArray(contents);
    }

    /// <summary>
    ///     Print Program Information.
    /// </summary>
    /// <param name="io">IOServer to print at.</param>
    /// <param name="assemblyName">Name of the Assembly</param>
    /// <param name="assemblyVersion">Version of the Assembly</param>
    /// <param name="showMobileSuitPowered">Show "Powered by MobileSuit or not"</param>
    /// <param name="owner">Optional. Owner of the Assembly, You may add 'All right reserved' manually.</param>
    /// <param name="site">Optional. Site of the Assembly</param>
    /// <param name="showLsHelp">Optional. Show Ls usage help or not</param>
    public static void PrintAssemblyInformation
    (
        this IIOHub io,
        string assemblyName,
        Version? assemblyVersion,
        bool showMobileSuitPowered = false,
        string? owner = null,
        string? site = null,
        bool showLsHelp = true
    )
    {
        if (io == null) return;

        if (showMobileSuitPowered)
            io.WriteLine
            (
                SuitUtils.CreateContentArray
                (
                    (assemblyName, null),
                    (" ", null),
                    (assemblyVersion?.ToString() ?? "", io.ColorSetting.TitleColor),
                    (" ", null),
                    (Lang.PoweredBy, null),
                    ("MobileSuit ", io.ColorSetting.ErrorColor),
                    (Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "",
                     io.ColorSetting.TitleColor)
                )
            );
        else
            io.WriteLine
            (
                SuitUtils.CreateContentArray
                (
                    (assemblyName, null),
                    (" ", null),
                    (assemblyVersion?.ToString() ?? "", io.ColorSetting.TitleColor)
                )
            );
        io.WriteLine();
        if (owner != null)
            io.WriteLine
            (
                SuitUtils.CreateContentArray
                (
                    (Lang.CopyrightC, null),
                    (owner, io.ColorSetting.TitleColor)
                    //, (Lang.AllRightsReserved, null)
                )
            );
        io.WriteLine();
        if (site != null)
        {
            io.WriteLine(site, io.ColorSetting.InformationColor);
            io.WriteLine();
        }

        if (showLsHelp)
        {
            io.WriteLine
            (
                SuitUtils.CreateContentArray
                (
                    (Lang.LsHelp1, null),
                    ("Ls", io.ColorSetting.PromptColor),
                    (Lang.LsHelp2, null)
                )
            );
            io.WriteLine();
        }
    }

    /// <summary>
    ///     Print powered by MobileSuit Information.
    /// </summary>
    /// <param name="io">IOServer to print at.</param>
    public static void PrintMobileSuitInformation(this IIOHub io)
    {
        if (io == null) return;
        io.WriteLine
        (
            SuitUtils.CreateContentArray
            (
                (Lang.PoweredBy, null),
                ("MobileSuit", io.ColorSetting.ErrorColor),
                (" ", null),
                (Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "", io.ColorSetting.TitleColor)
            )
        );
        io.WriteLine
        (
            SuitUtils.CreateContentArray
            (
                (Lang.CopyrightC, null),
                ("HIT-ReFreSH", io.ColorSetting.TitleColor)
                //, (Lang.AllRightsReserved, null)
            )
        );
        io.WriteLine();
        io.WriteLine("https://ms.ifers.xyz", io.ColorSetting.InformationColor);
        io.WriteLine
        (
            SuitUtils.CreateContentArray
            (
                (Lang.LsHelp1, null),
                ("Help", io.ColorSetting.PromptColor),
                (Lang.MsHelp2, null)
            )
        );

        io.WriteLine();
    }
}