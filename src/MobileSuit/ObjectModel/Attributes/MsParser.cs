using System;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class MsParserAttribute : Attribute
    {
        public MsParserAttribute(Converter<string, object> converter)
        {
            Converter = converter;
        }

        public Converter<string, object> Converter { get; }
    }
}