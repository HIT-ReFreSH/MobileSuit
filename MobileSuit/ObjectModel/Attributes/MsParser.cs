using System;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class MsParserAttribute : Attribute
    {
        public Converter<string,object> Converter { get; private set; }
        public MsParserAttribute(Converter<string, object> converter)
        {
            Converter = converter;
            

        }


    }
}
