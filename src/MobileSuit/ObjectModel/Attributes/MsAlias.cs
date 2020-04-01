using System;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    /// <summary>
    /// Alias for a MsObject's member
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class MsAliasAttribute : Attribute
    {
        /// <summary>
        /// Initialize a MsAlias with its text.
        /// </summary>
        /// <param name="text">The alias.</param>
        public MsAliasAttribute(string text)
        {
            Text = text;
        }
        /// <summary>
        /// The alias.
        /// </summary>
        public string Text { get; }
    }
}