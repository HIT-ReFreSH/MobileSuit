using System;
using System.Collections.Generic;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false)]
    public sealed class MobileSuitInfoAttribute: Attribute, IInfoProvider
    {
        public string Prompt { get; private set; }
        public MobileSuitInfoAttribute(string prompt)
        {
            Prompt = prompt;
        }
    }
}
