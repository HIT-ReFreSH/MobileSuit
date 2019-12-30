using System;
using System.Collections.Generic;
using System.Text;

namespace MobileSuit
{
    [AttributeUsage(AttributeTargets.Class |
                       AttributeTargets.Struct,
                       AllowMultiple = true)]
    public class MobileSuitItem: Attribute
    {
    }
}
