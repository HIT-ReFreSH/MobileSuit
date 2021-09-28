using System;
using System.Collections.Generic;
using System.Text.Json;
using PlasticMetal.MobileSuit.Parsing;

namespace PlasticMetal.MobileSuit.Core.Services
{
    /// <summary>
    /// Get parsers for certain type.
    /// </summary>
    public interface IParsingService
    {
        /// <summary>
        /// Get a parser for certain type with certain name.
        /// </summary>
        /// <param name="type">certain type</param>
        /// <param name="name">certain name</param>
        /// <returns></returns>
        public Converter<string, object?> Get(Type type, string name = "");
    }

    internal class ParsingService : IParsingService
    {
        private Dictionary<Type, Dictionary<string, Converter<string, object?>>> _parsers = new();
        public void Add(SuitParser parser)
        {
            if (!_parsers.ContainsKey(parser.TargetType)) _parsers.Add(parser.TargetType, new());
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
