using System;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class MsInfoAttribute : Attribute, IInfoProvider
    {
        public MsInfoAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}