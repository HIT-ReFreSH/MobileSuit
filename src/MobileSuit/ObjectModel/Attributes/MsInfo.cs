using System;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    /// <summary>
    /// Stores the information of a member to be displayed.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class MsInfoAttribute : Attribute, IInfoProvider
    {
        /// <summary>
        /// Initialize with the information.
        /// </summary>
        /// <param name="text">The information.</param>
        public MsInfoAttribute(string text)
        {
            Text = text;
        }
        /// <summary>
        /// The information.
        /// </summary>
        public string Text { get; }
    }
}