using System;
using System.Drawing;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit.Themes;

/// <summary>
/// Solarized dark color theme for MobileSuit.
/// Official: https://ethanschoonover.com/solarized/
/// </summary>
public class SolarizedDarkTheme : IColorSetting
{
    public SolarizedDarkTheme()
    {
        // 根据 Solarized 官方调色板设置
        // 由于规格没有提供具体值，使用官方 Solarized Dark 调色板
        DefaultColor = Color.FromArgb(131, 148, 150);     // Base0 (规格中的 InputColor)
        InformationColor = Color.FromArgb(147, 161, 161); // Base1 (规格中的 OutputColor)
        ErrorColor = Color.FromArgb(220, 50, 47);         // Red (规格中的 ErrorColor)
        WarningColor = Color.FromArgb(203, 75, 22);       // Orange (规格中的 WarningColor)
        OkColor = Color.FromArgb(133, 153, 0);            // Green (规格中的 SuccessColor)

        // 使用 Solarized 调色板中的蓝色作为 PromptColor
        PromptColor = Color.FromArgb(38, 139, 210);       // Blue (规格中的 PromptColor)

        // 使用 Solarized 调色板中的青色作为 InfoColor（映射到 TitleColor）
        TitleColor = Color.FromArgb(42, 161, 152);        // Cyan (规格中的 InfoColor)

        // MobileSuit 额外需要的颜色
        SystemColor = Color.FromArgb(0, 43, 54);          // Base03
        BackgroundColor = Color.FromArgb(0, 43, 54);      // Base03
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