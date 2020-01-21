using System;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public sealed class MsAliasAttribute : Attribute
    {
        // This is a positional argument
        public MsAliasAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
