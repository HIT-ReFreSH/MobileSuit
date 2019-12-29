using System;
using System.Collections.Generic;
using System.Text;

namespace MobileSuit.ObjectModel
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class MobileSuitIgnoreAttribute:Attribute
    {
    }
}
