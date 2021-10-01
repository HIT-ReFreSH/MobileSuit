using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PlasticMetal.MobileSuit.Core.Services
{
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
        public void Add(SuitParser parser);
    }

    internal class ParsingService : IParsingService
    {
        private readonly Dictionary<Type, Dictionary<string, Converter<string, object?>>> _parsers = new();

        public ParsingService()
        {
            Add(byte.Parse);
            Add(int.Parse);
            Add(short.Parse);
            Add(long.Parse);
            Add(sbyte.Parse);
            Add(uint.Parse);
            Add(ushort.Parse);
            Add(ulong.Parse);
            Add(double.Parse);
            Add(float.Parse);
            Add(decimal.Parse);
            Add(bool.Parse);
            Add(char.Parse);
            Add(DateTime.Parse);
        }

        public void Add<T>(Converter<string, T> converter, string name = "")
        {
            Add(SuitParser.FromConverter(converter, name));
        }

        public void Add(SuitParser parser)
        {
            if (!_parsers.ContainsKey(parser.TargetType))
                _parsers.Add(parser.TargetType, new Dictionary<string, Converter<string, object?>>());
            if (_parsers[parser.TargetType].ContainsKey(parser.Name)) _parsers[parser.TargetType].Remove(parser.Name);
            _parsers[parser.TargetType].Add(parser.Name, parser.Parser);
        }

        public Converter<string, object?> Get(Type type, string name = "")
        {
            if (_parsers.TryGetValue(type, out var nameDic) && nameDic.TryGetValue(name, out var p))
                return p;
            return s => JsonSerializer.Deserialize(s, type);
        }
    }
}