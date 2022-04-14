using System;
using System.Reflection;

namespace PlasticMetal.MobileSuit;

/// <summary>
///     Stores a parser which convert string argument to certain type.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class SuitParserAttribute : Attribute
{
    /// <summary>
    ///     Initialize with a parser.
    /// </summary>
    /// <param name="parserClass">The class which contains the parser method(A public static method)</param>
    /// <param name="name">The parser name, name of injected parser, ot parser class's name</param>
    public SuitParserAttribute(string name, Type? parserClass = null)
    {
        Name = name;
        Converter = parserClass?.GetMethod(name,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
            ?.CreateDelegate(typeof(Converter<string, object>)) as Converter<string, object>;
    }

    /// <summary>
    ///     The parser name, name of injected parser, ot parser class's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     The parser which convert string argument to certain type.
    /// </summary>
    public Converter<string, object?>? Converter { get; }
}