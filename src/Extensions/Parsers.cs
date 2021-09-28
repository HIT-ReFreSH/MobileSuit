using System.Globalization;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Provides most often used parsers.
    /// </summary>
    public static class Parsers
    {
        /// <summary>
        ///     int.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseInt(string arg)
        {
            return int.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     uint.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseUInt(string arg)
        {
            return uint.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     long.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseLong(string arg)
        {
            return long.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     ulong.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseULong(string arg)
        {
            return ulong.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     short.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseShort(string arg)
        {
            return short.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     ushort.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseUShort(string arg)
        {
            return ushort.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     double.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseDouble(string arg)
        {
            return double.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     float.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseFloat(string arg)
        {
            return float.Parse(arg, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     decimal.Parse
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object ParseDecimal(string arg)
        {
            return decimal.Parse(arg, CultureInfo.CurrentCulture);
        }
    }
}