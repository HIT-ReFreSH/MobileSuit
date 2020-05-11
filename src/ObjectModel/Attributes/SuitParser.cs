using System;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    /// <summary>
    ///     Stores a parser which convert string argument to certain type.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class SuitParserAttribute : Attribute
    {
        /// <summary>
        ///     Initialize with a parser.
        /// </summary>
        /// <param name="converter">The parser which convert string argument to certain type.</param>
        public SuitParserAttribute(Converter<string, object> converter)
        {
            Converter = converter;
        }

        /// <summary>
        ///     The parser which convert string argument to certain type.
        /// </summary>
        public Converter<string, object> Converter { get; }
    }
}