#nullable enable

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     Represents a Parameter which can be parsed from a String[].
    /// </summary>
    public interface IDynamicParameter
    {
        /// <summary>
        ///     Parse this Parameter from String[].
        /// </summary>
        /// <param name="options">String[] to parse from.</param>
        /// <returns>Whether the parsing is successful</returns>
        bool Parse(string[]? options = null);
    }
}