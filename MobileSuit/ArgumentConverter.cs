using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ArgumentConverterAttribute : Attribute
    {
        public Converter<string,object> Converter { get; private set; }
        public ArgumentConverterAttribute(Converter<string, object> converter)
        {
            Converter = converter;
            

        }


    }
}
