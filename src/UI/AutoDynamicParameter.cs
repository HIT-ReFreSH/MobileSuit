using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit.UI;

/// <summary>
///     A DynamicParameter which can parse itself automatically
/// </summary>
public abstract class AutoDynamicParameter : IDynamicParameter
{
    /// <summary>
    ///     Initialize a AutoDynamicParameter
    /// </summary>
    protected AutoDynamicParameter()
    {
        var myType = GetType();
        foreach (var property in myType.GetProperties(SuitObjectShell.Flags))
        {
            var memberAttr = property.GetCustomAttribute<ParsingMemberAttribute>(true);
            if (memberAttr is null) continue;
            var parseAttr = property.GetCustomAttribute<SuitParserAttribute>(true);

            Members.Add(memberAttr.Name, new ParsingMember(
                property.GetCustomAttribute<AsCollectionAttribute>(true) != null
                    ? new Action<object?, object?>((obj, value) =>
                        {
                            property.PropertyType.GetMethod("Add", new[]
                            {
                                property.GetType().GetElementType() ?? typeof(string)
                            })?.Invoke(property.GetValue(obj), new[] { value });
                        }
                    )
                    : property.SetValue,
                SuitBuildUtils.CreateConverterFactory(property.PropertyType, parseAttr),
                memberAttr.Length,
                property.GetCustomAttribute<WithDefaultAttribute>(true) != null));
        }
    }

    /// <summary>
    ///     Members of this AutoDynamicParameter
    /// </summary>
    private Dictionary<string, ParsingMember> Members { get; } = new();

    private static Regex ParseMemberRegex { get; } = new(@"^-");

    /// <inheritdoc />
    public bool Parse(IReadOnlyList<string> args, SuitContext context)
    {
        if (args is not { Count: > 0 })
            return Members.Values.All(member => member.Assigned);
        for (var i = 0; i < args.Count;)
        {
            if (!ParseMemberRegex.IsMatch(args[i]))
                throw new ArgumentException(
                    $@"{args[i]}{Lang.AutoDynamicParameter_Parse__0__not_match_format______}: '^-'",
                    nameof(args));

            var name = args[i][1..];
            if (!Members.ContainsKey(name))
                throw new ArgumentException(
                    $@"{args[i]}{Lang.AutoDynamicParameter_Parse__0__not_in_dictionary___1__}:{{{string.Join(',', Members.Keys)}}}",
                    nameof(args));

            var parseMember = Members[name];
            i++;
            var j = i + parseMember.ParseLength;
            if (j > args.Count)
                throw new ArgumentException($@"{args[i]}{Lang.AutoDynamicParameter_Parse__0__length_not_match}",
                    nameof(args));

            parseMember.Set(this,
                ConnectStringArray(args.ToArray()[i..j]), context);
            i = j;
        }

        return Members.Values.All(member => member.Assigned);
    }

    private static string ConnectStringArray(string[] array)
    {
        if (array.Length == 0) return "";
        var r = array[0];
        if (array.Length <= 1) return r;
        for (var i = 1; i < array.Length; i++)
            r += ' ' + array[i];
        return r;
    }


    private class ParsingMember
    {
        public ParsingMember(Action<object?, object?> setter,
            Func<SuitContext, Converter<string, object?>> converterFactory, int parseLength, bool withDefault
        )
        {
            Setter = setter;
            GetConverter = converterFactory;
            ParseLength = parseLength;
            Assigned = withDefault || parseLength == 0;
        }

        private Action<object?, object?> Setter { get; }
        private Func<SuitContext, Converter<string, object?>> GetConverter { get; }

        public bool Assigned { get; private set; }
        public int ParseLength { get; }

        public void Set(AutoDynamicParameter instance, string value, SuitContext context)
        {
            Setter(instance, ParseLength == 0 ? true : GetConverter(context)(value));
            Assigned = true;
        }
    }
}