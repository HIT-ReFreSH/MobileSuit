using System;


namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     A switch used in a dynamic parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SwitchAttribute : ParsingMemberAttribute
    {
        /// <summary>
        ///     Initialize a Option with name
        /// </summary>
        /// <param name="name">The name of option, for '-a' option, it's 'a'</param>
        public SwitchAttribute(string name) : base(name, 0)
        {
        }
    }
}