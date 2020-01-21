using System;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false)]
    public sealed class MsInfoAttribute: Attribute, IInfoProvider
    {
        public string Text { get; private set; }
        public MsInfoAttribute(string text)
        {
            Text = text;
        }
    }
}
