using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.VisualBasic;
using System.Reflection;

using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PlasticMetal.MobileSuit.ObjectModel.Parsing
{
    /// <summary>
    /// A DynamicParameter which can parse itself automatically
    /// </summary>
    public abstract class AutoDynamicParameter : IDynamicParameter
    {
        /// <summary>
        /// Members of this AutoDynamicParameter
        /// </summary>
        private Dictionary<string, ParsingMember> Members { get; } = new Dictionary<string, ParsingMember>();
        /// <summary>
        /// Initialize a AutoDynamicParameter
        /// </summary>
        protected AutoDynamicParameter()
        {
            var myType = this.GetType();
            foreach (var property in myType.GetProperties(SuitObject.Flags))
            {
                if (!(property.GetCustomAttribute(typeof(ParsingMemberAttribute), true) is ParsingMemberAttribute memberAttr)) continue;
                var parseAttr = property.GetCustomAttribute(typeof(SuitParserAttribute), true) as SuitParserAttribute;
                Members.Add(memberAttr.Name, new ParsingMember(property.SetValue,
                    parseAttr?.Converter ?? (a => a),
                    memberAttr.Name,
                    memberAttr.Length,
                    property.GetCustomAttribute(typeof(WithDefaultAttribute)) != null));
            }
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
            private Action<object?, object?> Setter { get; }
            private Converter<string, object> Converter { get; }

            public bool Assigned { get; private set; }
            public int ParseLength { get; }
            public string Name { get; }

            public void Set(AutoDynamicParameter instance, string value)
            {
                Setter(instance, ParseLength == 0 ? true : Converter(value));
                Assigned = true;
            }

            public ParsingMember(Action<object?, object?> setter,
                Converter<string, object> converter, string name, int parseLength, bool withDefault
                )
            {
                Setter = setter;
                Converter = converter;
                Name = name;
                ParseLength = parseLength;
                Assigned = withDefault || parseLength == 0;
            }
        }

        private static Regex ParseMemberRegex { get; } = new Regex("^-\\w+$");

        /// <inheritdoc />
        public bool Parse(string[]? options = null)
        {
            if (options != null && options.Length > 0)
            {
                for (var i = 0; i < options.Length;)
                {
                    if (!ParseMemberRegex.IsMatch(options[i])) return false;
                    var name = options[i][1..];
                    if (!Members.ContainsKey(name)) return false;
                    var parseMember = Members[name];
                    i++;
                    var j = i + parseMember.ParseLength;
                    if (j > options.Length) return false;
                    parseMember.Set(this,
                        ConnectStringArray(options[i..j] ?? Array.Empty<string>()));
                    i = j;
                }
            }
            return Members.Values.All(member => member.Assigned);
        }
    }
}
