using System;
using System.Drawing;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit.Themes;

/// <summary>
/// Dracula color theme for MobileSuit.
/// Official: https://draculatheme.com/contribute
/// </summary>
public class DraculaTheme : IColorSetting
{
    public DraculaTheme()
    {
        // 按照规格设置
        PromptColor = Color.FromArgb(189, 147, 249);      // Purple
        DefaultColor = Color.FromArgb(248, 248, 242);     // Foreground (作为InputColor)
        InformationColor = Color.FromArgb(248, 248, 242); // Foreground (作为OutputColor)
        ErrorColor = Color.FromArgb(255, 85, 85);         // Red
        WarningColor = Color.FromArgb(241, 250, 140);     // Yellow
        OkColor = Color.FromArgb(80, 250, 123);           // Green (作为SuccessColor)
        TitleColor = Color.FromArgb(255, 121, 198);       // Pink (补充)

        // 补充其他必要的颜色
        SystemColor = Color.FromArgb(68, 71, 90);         // Current Line
        BackgroundColor = Color.FromArgb(40, 42, 54);     // Background
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