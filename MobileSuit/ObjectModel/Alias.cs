using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public sealed class AliasAttribute : Attribute
    {
        // This is a positional argument
        public AliasAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
