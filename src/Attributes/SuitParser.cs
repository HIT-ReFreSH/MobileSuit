using HitRefresh.MobileSuit.Core;
using System;
using System.Reflection;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Describes a parser which convert string argument to certain type.
/// </summary>
public interface IParsingInfoProvider<out T>
{
    /// <summary>
    ///     The parser name, name of injected parser, ot parser class's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     The parser which convert string argument to certain type.
    /// </summary>
    public Converter<string, T?>? Converter{ get; }
}
/// <summary>
///     Stores a parser which convert string argument to certain type.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class SuitParserAttribute : Attribute, IParsingInfoProvider<object>
{
    /// <summary>
    ///     Initialize with a parser.
    /// </summary>
    /// <param name="name">The parser name, name of injected parser, or parser method's name; "Parser" is default.</param>
    public SuitParserAttribute(string name = "Parse")
    {
        Name=name;
    }

    /// <summary>
    ///     The parser name, name of injected parser, ot parser class's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     The parser which convert string argument to certain type.
    /// </summary>
    public Converter<string, object?>? Converter => null;
}
/// <summary>
///     Stores a parser which convert string argument to certain type.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class SuitParserAttribute<TTarget> : SuitParserAttribute<TTarget,TTarget>
{
    /// <summary>
    ///     Initialize with a parser.
    /// </summary>
    /// <param name="name">The parser name, name of injected parser, or parser method's name; "Parser" is default.</param>
    public SuitParserAttribute(string name = "Parse") : base(name)
    {

    }
}
/// <summary>
///     Stores a parser which convert string argument to certain type.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class SuitParserAttribute<TConverter,TTarget> : Attribute, IParsingInfoProvider<TTarget>
{
    /// <summary>
    ///     Initialize with a parser.
    /// </summary>
    /// <param name="name">The parser name, name of injected parser, or parser method's name; "Parser" is default.</param>
    public SuitParserAttribute(string name="Parse")
    {
        Name = name;
        Converter = typeof(TConverter).GetMethod(name,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
            ?.CreateDelegate(typeof(Converter<string, TTarget>)) as Converter<string, TTarget>;
    }

    /// <summary>
    ///     The parser name, name of injected parser, ot parser class's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     The parser which convert string argument to certain type.
    /// </summary>
    public Converter<string, TTarget?>? Converter { get; }
}