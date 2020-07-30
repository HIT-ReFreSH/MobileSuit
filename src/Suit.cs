using System;
using System.Collections.Generic;
using System.Text;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// Providing Common APIs for MobileSuit
    /// </summary>
    public static class Suit
    {
        /// <summary>
        /// Get a builder to create host
        /// </summary>
        /// <returns>The builder</returns>
        public static SuitHostBuilder GetBuilder() => new SuitHostBuilder();
        /// <summary>
        ///     Default IOServer, using stdin, stdout, stderr.
        /// </summary>
        public static IOServer GeneralIO { get; set; } = new IOServer();

        /// <summary>
        ///     provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?)> CreateContentArray(params (string, ConsoleColor?)[] contents)
            => IIOServer.CreateContentArray(contents);


        /// <summary>
        ///     provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?, ConsoleColor?)> CreateContentArray(
            params (string, ConsoleColor?, ConsoleColor?)[] contents)
            => IIOServer.CreateContentArray(contents);
    }
}
