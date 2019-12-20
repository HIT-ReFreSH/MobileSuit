using System;
using System.Collections.Generic;
using System.Text;

namespace MobileSuit
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class MobileSuitDataConverterAttribute : Attribute
    {
        public Converter<string,object> Converter { get; private set; }
        public MobileSuitDataConverterAttribute(Converter<string, object> converter)
        {
            Converter = converter;
            

        }


    }
}
