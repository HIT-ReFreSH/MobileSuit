using System;
using System.Collections.Generic;
using System.Reflection;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Providing Common APIs for MobileSuit
    /// </summary>
    public static class Suit
    {
        /// <summary>
        ///     Default IOServer, using stdin, stdout, stderr.
        /// </summary>
        public static IOServer GeneralIO { get; set; } = new IOServer();

        /// <summary>
        ///     Get a builder to create host
        /// </summary>
        /// <returns>The builder</returns>
        public static SuitHostBuilder GetBuilder()
        {
            return new SuitHostBuilder();
        }

        /// <summary>
        ///     provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?)> CreateContentArray(params (string, ConsoleColor?)[] contents)
        {
            return IIOServer.CreateContentArray(contents);
        }


        /// <summary>
        ///     provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?, ConsoleColor?)> CreateContentArray(
            params (string, ConsoleColor?, ConsoleColor?)[] contents)
        {
            return IIOServer.CreateContentArray(contents);
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
        public static void PrintAssemblyInformation(this IIOServer io,
            string assemblyName, Version? assemblyVersion,
            bool showMobileSuitPowered = false,
            string? owner = null,
            string? site = null,
            bool showLsHelp = true
        )
        {
            if (io == null) return;

            if (showMobileSuitPowered)
                io.WriteLine(CreateContentArray(
                    (assemblyName, null),
                    (" ", null),
                    (assemblyVersion?.ToString() ?? "", io.ColorSetting.ListTitleColor),
                    (" ", null),
                    (Lang.PoweredBy, null),
                    ("MobileSuit(", io.ColorSetting.ErrorColor),
                    ("https://ms.ifers.xyz", io.ColorSetting.CustomInformationColor),
                    (") ", null),
                    (Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "",
                        io.ColorSetting.ListTitleColor)
                ));
            else
                io.WriteLine(CreateContentArray(
                    (assemblyName, null),
                    (" ", null),
                    (assemblyVersion?.ToString() ?? "", io.ColorSetting.ListTitleColor)
                ));
            io.WriteLine();
            if (owner != null)
                io.WriteLine(CreateContentArray(
                    (Lang.CopyrightC, null),
                    (owner, io.ColorSetting.ListTitleColor)
                    //, (Lang.AllRightsReserved, null)
                ));
            io.WriteLine();
            if (site != null)
            {
                io.WriteLine(site, io.ColorSetting.CustomInformationColor);
                io.WriteLine();
            }

            if (showLsHelp)
            {
                io.WriteLine(CreateContentArray(
                    (Lang.LsHelp1, null),
                    ("Ls", io.ColorSetting.PromptColor),
                    (Lang.LsHelp2, null)
                ));
                io.WriteLine();
            }
        }

        /// <summary>
        ///     Print powered by MobileSuit Information.
        /// </summary>
        /// <param name="io">IOServer to print at.</param>
        public static void PrintMobileSuitInformation(this IIOServer io)
        {
            if (io == null) return;
            io.WriteLine(CreateContentArray(
                (Lang.PoweredBy, null),
                ("MobileSuit", io.ColorSetting.ErrorColor),
                (" ", null),
                (Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "", io.ColorSetting.ListTitleColor)
            ));
            io.WriteLine(CreateContentArray(
                (Lang.CopyrightC, null),
                ("Plastic-Metal", io.ColorSetting.ListTitleColor)
                //, (Lang.AllRightsReserved, null)
            ));
            io.WriteLine();
            io.WriteLine("https://ms.ifers.xyz", io.ColorSetting.CustomInformationColor);
            io.WriteLine(CreateContentArray(
                (Lang.LsHelp1, null),
                ("Help", io.ColorSetting.PromptColor),
                (Lang.MsHelp2, null)
            ));

            io.WriteLine();
        }
    }
}