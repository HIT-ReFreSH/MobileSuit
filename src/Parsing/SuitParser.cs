using System;
using System.Reflection;

namespace PlasticMetal.MobileSuit.Parsing
{
    /// <summary>
    ///     Stores a parser which convert string argument to certain type.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SuitParserAttribute : Attribute
    {
        /// <summary>
        ///     Initialize with a parser.
        /// </summary>
        /// <param name="parserClass">The class which contains the parser method</param>
        /// <param name="methodName">The parser method, which MUST BE public static</param>
        public SuitParserAttribute(Type parserClass, string methodName)
        {
            if (parserClass == null || methodName == null) return;
            Converter= parserClass.GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Static|BindingFlags.IgnoreCase)
                ?.CreateDelegate(typeof( Converter<string, object>)) as Converter<string, object>;
        }

        /// <summary>
        ///     The parser which convert string argument to certain type.
        /// </summary>
        public Converter<string, object>? Converter { get; }
    }
}