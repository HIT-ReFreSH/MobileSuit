using System;

namespace PlasticMetal.MobileSuit.Parsing
{
    /// <summary>
    ///     A member of a dynamic parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ParsingMemberAttribute : Attribute
    {
        /// <summary>
        ///     Initialize a
        /// </summary>
        /// <param name="name">The name of option, for '-a', it's 'a'.</param>
        /// <param name="length">The length of String[] used to parse this arg</param>
        protected ParsingMemberAttribute(string name, int length)
        {
            Name = name;
            Length = length;
        }

        /// <summary>
        ///     The name of option, for '-a', it's 'a'.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The length of String[] used to parse this arg
        /// </summary>
        public int Length { get; }
    }
}