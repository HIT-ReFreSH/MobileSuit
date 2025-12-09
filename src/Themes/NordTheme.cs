using System;
using System.Drawing;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit.Themes;

/// <summary>
/// Nord color theme for MobileSuit.
/// Official: https://www.nordtheme.com/docs/colors-and-palettes
/// </summary>
public class NordTheme : IColorSetting
{
    public NordTheme()
    {
        // 按照规格设置
        PromptColor = Color.FromArgb(136, 192, 208);      // Nord8 - Bright blue
        DefaultColor = Color.FromArgb(216, 222, 233);     // Nord6 - Light gray (作为InputColor)
        InformationColor = Color.FromArgb(236, 239, 244); // Nord4 - White (作为OutputColor/InfoColor)
        ErrorColor = Color.FromArgb(191, 97, 106);        // Nord11 - Red
        WarningColor = Color.FromArgb(235, 203, 139);     // Nord13 - Yellow
        OkColor = Color.FromArgb(163, 190, 140);          // Nord14 - Green (作为SuccessColor)
        TitleColor = Color.FromArgb(94, 129, 172);        // Nord10 - Frost (补充)

        // 补充其他必要的颜色
        SystemColor = Color.FromArgb(46, 52, 64);         // Nord0 - Polar night
        BackgroundColor = Color.FromArgb(46, 52, 64);     // Nord0 - Polar night
    }

    public Color DefaultColor { get; set; }
    public Color PromptColor { get; set; }
    public Color ErrorColor { get; set; }
    public Color OkColor { get; set; }
    public Color TitleColor { get; set; }
    public Color InformationColor { get; set; }
    public Color SystemColor { get; set; }
    public Color WarningColor { get; set; }
    public Color BackgroundColor { get; set; }

    public bool Equals(IColorSetting? other)
    {
        if (other is null) return false;
        return DefaultColor == other.DefaultColor &&
               PromptColor == other.PromptColor &&
               ErrorColor == other.ErrorColor &&
               OkColor == other.OkColor &&
               TitleColor == other.TitleColor &&
               InformationColor == other.InformationColor &&
               SystemColor == other.SystemColor &&
               WarningColor == other.WarningColor &&
               BackgroundColor == other.BackgroundColor;
    }

    public override bool Equals(object? that) => that is IColorSetting other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new System.HashCode();
        hash.Add(DefaultColor);
        hash.Add(PromptColor);
        hash.Add(ErrorColor);
        hash.Add(OkColor);
        hash.Add(TitleColor);
        hash.Add(InformationColor);
        hash.Add(SystemColor);
        hash.Add(WarningColor);
        hash.Add(BackgroundColor);
        return hash.ToHashCode();
    }
}