using System;

namespace PlasticMetal.MobileSuit.Parsing
{
    /// <summary>
    ///     A option used in a dynamic parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class OptionAttribute : ParsingMemberAttribute
    {
        /// <summary>
        ///     The length of array which will be used to parse this option.
        /// </summary>
        public OptionAttribute(string option) : this(option, 1)
        {
        }

        /// <summary>
        ///     Initialize a Option with name and parse length
        /// </summary>
        /// <param name="option">The name of option, for '-a' option, it's 'a'</param>
        /// <param name="parseLength">The length of array which will be used to parse this option.</param>
        public OptionAttribute(string option, int parseLength) : base(option, parseLength)
        {
        }
    }
}