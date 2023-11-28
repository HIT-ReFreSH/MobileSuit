using System;
using System.Collections.Generic;
using System.Reflection;

namespace HitRefresh.MobileSuit.Core;

/// <summary>
///     A data parser of MobileSuit
/// </summary>
public class SuitParser<T>
{
    private SuitParser(string name, Converter<string, T?> parser)
    {
        Name = name;
        Parser = parser;
    }



    /// <summary>
    ///     Name of the Parser
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The parser which convert string argument to certain type.
    /// </summary>
    public Converter<string, T?> Parser { get; }

    /// <summary>
    ///     Create a mobile suit parser from a converter
    /// </summary>
    /// <param name="converter">The converter method</param>
    /// <param name="name">Name of the parser, if set empty, the parser will be default.</param>
    /// <returns></returns>
    public static SuitParser<T> FromConverter(Converter<string, T> converter, string name = "")
    {
        return new SuitParser<T>( name, converter);
    }

    /// <summary>
    ///     Create a mobile suit parser from a converter
    /// </summary>

    /// <param name="name">Name of the parser, if set empty, the parser will be default.</param>
    /// <returns></returns>
    public static SuitParser<T> FromName(string name)
        => FromConverter((typeof(T).GetMethod("Parse",
                                 BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase, new[]{typeof(string)})
                             ?.CreateDelegate(typeof(Converter<string, T>)) as Converter<string, T>) ??
                         throw new KeyNotFoundException("Parse"), name);
    /// <summary>
    ///     Create a mobile suit parser from a converter
    /// </summary>
    /// <returns></returns>
    public static SuitParser<T> From() => FromName("");
}