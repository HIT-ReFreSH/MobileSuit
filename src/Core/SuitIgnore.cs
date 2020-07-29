using System;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Represents that this member should be ignored by Mobile Suit.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SuitIgnoreAttribute : Attribute
    {
    }
}