using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PlasticMetal.MobileSuit.Core.Services;

/// <summary>
///     Get parsers for certain type.
/// </summary>
public interface IParsingService
{
    /// <summary>
    ///     Get a parser for certain type with certain name.
    /// </summary>
    /// <param name="type">certain type</param>
    /// <param name="name">certain name</param>
    /// <returns></returns>
    public Converter<string, object?> Get(Type type, string name = "");

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="origin"></param>
    ///// <param name="parserName"></param>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //public bool TryParse<T>(string origin, string parserName ,out T value);
    /// <summary>
    ///     Add a parser with certain name to parsing service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="converter"></param>
    /// <param name="name"></param>
    public void Add<T>(Converter<string, T> converter, string name = "");

    /// <summary>
    ///     Add a parser.
    /// </summary>
    /// <param name="parser"></param>
    public void Add<T>(SuitParser<T> parser);

    /// <summary>
    ///     Add a parser.
    /// </summary>
    /// <param name="name"></param>
    public void Add<T>(string name = "");
    }

internal class ParsingService : IParsingService
{
    private readonly Dictionary<Type, Dictionary<string, Converter<string, object?>>> _parsers = new();

    public ParsingService()
    {
        Add<byte>();
        Add<short>();
        Add<int>();
        Add<long>();
        Add<float>();
        Add<double>();
        Add<decimal>();
        Add<sbyte>();
        Add<ushort>();
        Add<uint>();
        Add<ulong>();
        Add<bool>();
        Add<char>();
        Add<DateTime>();
        Add<TimeSpan>();
    }

    public void Add<T>(Converter<string, T> converter, string name = "")
    {
        Add(SuitParser<T>.FromConverter(converter, name));
    }

    public void Add<T>(SuitParser<T> parser)
    {
        if (!_parsers.ContainsKey(typeof(T)))
            _parsers.Add(typeof(T), new Dictionary<string, Converter<string, object?>>());
        if (_parsers[typeof(T)].ContainsKey(parser.Name)) _parsers[typeof(T)].Remove(parser.Name);
        _parsers[typeof(T)].Add(parser.Name, x=>parser.Parser(x));
    }

    public void Add<T>(string name = "")=>Add(SuitParser<T>.FromName(name));

    public Converter<string, object?> Get(Type type, string name = "")
    {
        if (_parsers.TryGetValue(type, out var nameDic) && nameDic.TryGetValue(name, out var p))
            return p;
        return s => JsonSerializer.Deserialize(s, type);
    }
}