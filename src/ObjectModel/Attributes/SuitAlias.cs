using System;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    /// <summary>
    ///     Alias for a SuitObject's member
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class SuitAliasAttribute : Attribute
    {
        /// <summary>
        ///     Initialize a SuitAlias with its text.
        /// </summary>
        /// <param name="text">The alias.</param>
        public SuitAliasAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        ///     The alias.
        /// </summary>
        public string Text { get; }
    }
}