using PlasticMetal.MobileSuit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static PlasticMetal.MobileSuit.SuitUtils;

namespace PlasticMetal.MobileSuit.UI;

/// <summary>
/// Useful input components for console UI.
/// </summary>
public static class ConsoleInput
{
    /// <summary>
    /// Serialize target with ToString()??""
    /// </summary>
    /// <typeparam name="T">Any type not null</typeparam>
    /// <param name="origin">Origin value</param>
    /// <returns>Serialized target used ToString()??""</returns>
    public static string ToStringOrEmptySerializer<T>(T origin)
        => origin?.ToString() ?? "";

    //public static T CuiValiqdatedReadline<T>
    //    (this IIOHub hub, string prompt, Func<string?, bool> validator)
    //{

    //}

    /// <summary>
    /// A CUI that allows the user select ONE objective from Alternative objectives.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hub">IIOHub</param>
    /// <param name="selectFrom">Alternative objectives</param>
    /// <param name="prompt">Prompt to guide user selection</param>
    /// <param name="labeler">Label of objectives to guide user selection </param>
    /// <param name="serializer">Method of Serializing Object as Text</param>
    /// <returns></returns>
    public static T CuiSelectItemFrom<T>
        (this IIOHub hub, string prompt, T[] selectFrom, Func<T, string>? serializer = null, Func<int, T, string>? labeler = null)
    {
        serializer ??= ToStringOrEmptySerializer<T>;
        labeler ??= (i, _) => i.ToString();
        var labels = new Dictionary<string, int>();
        for (; ; )
        {
            hub.WriteLine(prompt, OutputType.Title);
            var i = 0;
            foreach (var t in selectFrom)
            {
                var label = labeler(i, t);
                labels[label] = i;
                hub.WriteLine(CreateContentArray(
                    ($"{label}\t", null, hub.ColorSetting.TitleColor),
                    (serializer(t), hub.ColorSetting.DefaultColor, null)
                ));
                i++;
            }

            var ans = hub.ReadLine("", "0");
            if (ans is not null &&
                labels.TryGetValue(ans, out var value)) return selectFrom[value];
        }
    }


    /// <summary>
    /// A function to parse user input into several int index.
    /// </summary>
    public delegate int[]? ParseUserSelectItemsInput(string? userInput, IReadOnlyDictionary<string, int> labelMapping);

    /// <summary>
    /// Split user input using space and hyphen. E.g., "1-3 4" will be parsed to {1,2,3,4}.
    /// </summary>
    /// <param name="userInput">String from user</param>
    /// <param name="labelMapping">Dictionary maps label to index.</param>
    /// <returns></returns>
    public static int[]? SpaceHyphenParser(string? userInput, IReadOnlyDictionary<string, int> labelMapping)
    {
        try
        {
            return SpaceHyphenParserInternal(userInput, labelMapping).ToArray();
        }
        catch (SpaceHyphenParserException)
        {
            return null;
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }
    [Serializable]
    private class SpaceHyphenParserException : Exception
    {

        public SpaceHyphenParserException()
        {
        }

        public SpaceHyphenParserException(string message) : base(message)
        {
        }

        public SpaceHyphenParserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SpaceHyphenParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
    private static IEnumerable<int> SpaceHyphenParserInternal(string? userInput, IReadOnlyDictionary<string, int> labelMapping)
    {
        if (string.IsNullOrEmpty(userInput)) yield break;
        var inputGroups = userInput.Split(' ');
        foreach (var inputGroup in inputGroups)
        {
            var labels = inputGroup.Split("-").Select(l => labelMapping[l]).ToArray();
            switch (labels.Length)
            {
                case > 2 or 0:
                    throw new SpaceHyphenParserException();
                case 1:
                    yield return labels[0];
                    break;
                case 2:
                    var (lt, gt) = (labels[0], labels[1]);
                    if (gt < lt) throw new SpaceHyphenParserException();
                    for (var it = lt; it <= gt; it++) yield return it;
                    break;
            }
        }
    }

    /// <summary>
    /// A CUI that allows the user select ONE objective from Alternative objectives.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hub">IIOHub</param>
    /// <param name="selectFrom">Alternative objectives</param>
    /// <param name="prompt">Prompt to guide user selection</param>
    /// <param name="labeler">Label of objectives to guide user selection </param>
    /// <param name="serializer">Method of Serializing Object as Text</param>
    /// <param name="parser"></param>
    /// <returns></returns>
    public static IEnumerable<T> CuiSelectItemsFrom<T>
        (this IIOHub hub, string prompt, T[] selectFrom, Func<T, string>? serializer = null, Func<int, T, string>? labeler = null,
            ParseUserSelectItemsInput? parser = null)
    {
        serializer ??= ToStringOrEmptySerializer<T>;
        parser ??= SpaceHyphenParser;
        labeler ??= (i, _) => i.ToString();
        var labels = new Dictionary<string, int>();
        for (; ; )
        {
            hub.WriteLine(prompt, OutputType.Title);
            var i = 0;
            foreach (var t in selectFrom)
            {
                var label = labeler(i, t);
                labels[label] = i;
                hub.WriteLine(CreateContentArray(
                    ($"{label}\t", null, hub.ColorSetting.TitleColor),
                    (serializer(t), hub.ColorSetting.DefaultColor, null)
                ));
                i++;
            }

            var ans = parser( hub.ReadLine("", "0"),labels);
            if (ans != null) return ans.Select(value => selectFrom[value]);
        }
    }

    /// <summary>
    /// A CUI that allows the user answer yes/no by inputting "y" or "n".
    /// </summary>
    /// <param name="hub">IIOHub</param>
    /// <param name="prompt">Prompt to guide user input</param>
    /// <param name="default">NULL if only y/n is allowed; Otherwise, if user input is null or empty, such default value will be returned.</param>
    /// <returns>True if user says "yes".</returns>
    public static bool CuiYesNo(this IIOHub hub, string prompt, bool? @default=null)
    {
        for (;;)
        {
            var ans = hub.ReadLine(prompt)?.ToLower();
            switch (ans)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                case null or "":
                    if (@default is { } defaultReturn) return defaultReturn;
                    continue;
                default:
                    continue;
            }
        }
    }
}