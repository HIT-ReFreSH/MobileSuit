using System;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     A data parser of MobileSuit
    /// </summary>
    public class SuitParser
    {
        private SuitParser(Type targetType, string name, Converter<string, object?> parser)
        {
            TargetType = targetType;
            Name = name;
            Parser = parser;
        }

        /// <summary>
        ///     Type which string can be converted to.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///     Name of the Parser
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The parser which convert string argument to certain type.
        /// </summary>
        public Converter<string, object?> Parser { get; }

        /// <summary>
        ///     Create a mobile suit parser from a converter
        /// </summary>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <param name="converter">The converter method</param>
        /// <param name="name">Name of the parser, if set empty, the parser will be default.</param>
        /// <returns></returns>
        public static SuitParser FromConverter<T>(Converter<string, T> converter, string name = "")
        {
            return new SuitParser(typeof(T), name, s => converter(s));
        }
    }
}