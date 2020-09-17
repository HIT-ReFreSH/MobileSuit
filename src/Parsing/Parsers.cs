using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PlasticMetal.MobileSuit.Parsing
{
    /// <summary>
    /// Provides most often used parsers.
    /// </summary>
    public static class Parsers
    {
        /// <summary>
        /// int.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseInt(string arg) => int.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// uint.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseUInt(string arg) => uint.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// long.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseLong(string arg) => long.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// ulong.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseULong(string arg) => ulong.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// short.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseShort(string arg) => short.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// ushort.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseUShort(string arg) => ushort.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// double.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseDouble(string arg) => double.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// float.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseFloat(string arg) => float.Parse(arg, CultureInfo.CurrentCulture);
        /// <summary>
        /// decimal.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseDecimal(string arg) => decimal.Parse(arg, CultureInfo.CurrentCulture);

    }
}
