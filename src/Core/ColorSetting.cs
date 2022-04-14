using System;
using System.Drawing;

namespace PlasticMetal.MobileSuit.Core;

/// <summary>
///     Color settings of a Mobile Suit.
/// </summary>
public struct ColorSetting : IColorSetting, IEquatable<ColorSetting>
{
    /// <inheritdoc />
    public Color BackgroundColor { get; set; }

    /// <summary>
    ///     Default color. For OutputType.Default
    /// </summary>
    public Color DefaultColor { get; set; }

    /// <summary>
    ///     Prompt Color. For OutputType.Prompt
    /// </summary>
    public Color PromptColor { get; set; }

    /// <summary>
    ///     Prompt Color. For OutputType.Error
    /// </summary>
    public Color ErrorColor { get; set; }

    /// <summary>
    ///     Prompt Color. For OutputType.AllOK
    /// </summary>
    public Color OkColor { get; set; }

    /// <summary>
    ///     Prompt Color. For OutputType.ListTitle
    /// </summary>
    public Color TitleColor { get; set; }

    /// <summary>
    ///     Prompt Color. For OutputType.CustomInformation
    /// </summary>
    public Color InformationColor { get; set; }

    /// <summary>
    ///     Prompt Color. For OutputType.Information
    /// </summary>
    public Color SystemColor { get; set; }

    /// <inheritdoc />
    public Color WarningColor { get; set; }

    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <returns>whether this instance and a specified object are equal.</returns>
    public bool Equals(IColorSetting? other)
    {
        return other != null &&
               DefaultColor == other.DefaultColor && PromptColor == other.PromptColor &&
               ErrorColor == other.ErrorColor && OkColor == other.OkColor &&
               TitleColor == other.TitleColor && InformationColor == other.InformationColor &&
               SystemColor == other.SystemColor && WarningColor == other.WarningColor &&
               BackgroundColor == other.BackgroundColor;
    }

    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <param name="that">The object to compare with the current instance.</param>
    /// <returns>whether this instance and a specified object are equal.</returns>
    public override bool Equals(object? that)
    {
        return that is IColorSetting && Equals((ColorSetting) that);
    }

    /// <summary>
    ///     generate hash code of all colors
    /// </summary>
    /// <returns> hash code of all colors</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(HashCode.Combine(DefaultColor.GetHashCode(), PromptColor.GetHashCode(),
            ErrorColor.GetHashCode(), OkColor.GetHashCode(),
            TitleColor.GetHashCode(), InformationColor.GetHashCode(), SystemColor.GetHashCode(),
            WarningColor.GetHashCode()), BackgroundColor.GetHashCode());
    }

    /// <summary>
    ///     Indicates whether two instances are equal.
    /// </summary>
    /// <param name="left">left instance</param>
    /// <param name="right">right instance</param>
    /// <returns>true for equal</returns>
    public static bool operator ==(ColorSetting left, ColorSetting right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Indicates whether two instances are not-equal.
    /// </summary>
    /// <param name="left">left instance</param>
    /// <param name="right">right instance</param>
    /// <returns>true for not-equal</returns>
    public static bool operator !=(ColorSetting left, ColorSetting right)
    {
        return !(left == right);
    }

    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <returns>whether this instance and a specified object are equal.</returns>
    public bool Equals(ColorSetting other)
    {
        return Equals(other as IColorSetting);
    }
}