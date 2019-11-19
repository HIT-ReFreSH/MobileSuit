using System;
using System.Collections.Generic;
using System.Text;

namespace MobileSuit
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false)]
    public sealed class MobileSuitInfoAttribute: Attribute, IMobileSuitInfoProvider
    {
        public string Prompt { get; private set; }
        public MobileSuitInfoAttribute(string prompt)
        {
            Prompt = prompt;
        }
    }
}
