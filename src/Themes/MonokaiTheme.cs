using System;
using System.Drawing;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit.Themes;

/// <summary>
/// Monokai color theme for MobileSuit.
/// Based on Sublime Text's default theme.
/// </summary>
public class MonokaiTheme : IColorSetting
{
    public MonokaiTheme()
    {
        // Monokai 配色方案（Sublime Text 风格）
        DefaultColor = Color.FromArgb(248, 248, 242);     // Foreground - 前景色
        PromptColor = Color.FromArgb(249, 38, 114);       // Pink - 提示符
        InformationColor = Color.FromArgb(174, 129, 255); // Purple - 信息输出
        ErrorColor = Color.FromArgb(255, 0, 0);           // Red - 错误
        WarningColor = Color.FromArgb(230, 219, 116);     // Yellow - 警告
        OkColor = Color.FromArgb(166, 226, 46);           // Green - 成功
        TitleColor = Color.FromArgb(102, 217, 239);       // Cyan - 标题

        // 补充其他必要的颜色
        SystemColor = Color.FromArgb(39, 40, 34);         // Background - 系统/背景
        BackgroundColor = Color.FromArgb(39, 40, 34);     // Background - 背景
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