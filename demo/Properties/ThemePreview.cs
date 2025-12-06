using System;
using System.Drawing;
using HitRefresh.MobileSuit;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Themes;

namespace HitRefresh.MobileSuitDemo;

public static class ThemePreview
{
    public static void ShowTheme(IColorSetting theme, string themeName)
    {
        Console.WriteLine($"\n=== {themeName} Theme Preview ===");

        // 设置控制台颜色（如果支持）
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;

        // 显示颜色示例
        ShowColorBlock(theme.PromptColor, "Prompt");
        ShowColorBlock(theme.ErrorColor, "Error");
        ShowColorBlock(theme.OkColor, "Success");
        ShowColorBlock(theme.TitleColor, "Title");
        ShowColorBlock(theme.InformationColor, "Info");
        ShowColorBlock(theme.WarningColor, "Warning");
        ShowColorBlock(theme.SystemColor, "System");

        Console.WriteLine("\nRGB Values:");
        Console.WriteLine($"  Prompt:     {theme.PromptColor.R}, {theme.PromptColor.G}, {theme.PromptColor.B}");
        Console.WriteLine($"  Error:      {theme.ErrorColor.R}, {theme.ErrorColor.G}, {theme.ErrorColor.B}");
        Console.WriteLine($"  Success:    {theme.OkColor.R}, {theme.OkColor.G}, {theme.OkColor.B}");
        Console.WriteLine($"  Title:      {theme.TitleColor.R}, {theme.TitleColor.G}, {theme.TitleColor.B}");
        Console.WriteLine($"  Info:       {theme.InformationColor.R}, {theme.InformationColor.G}, {theme.InformationColor.B}");
        Console.WriteLine($"  Warning:    {theme.WarningColor.R}, {theme.WarningColor.G}, {theme.WarningColor.B}");
        Console.WriteLine($"  System:     {theme.SystemColor.R}, {theme.SystemColor.G}, {theme.SystemColor.B}");
        Console.WriteLine($"  Background: {theme.BackgroundColor.R}, {theme.BackgroundColor.G}, {theme.BackgroundColor.B}");
    }

    private static void ShowColorBlock(Color color, string label)
    {
        Console.Write($"{label,-10}: ");
        Console.BackgroundColor = ConvertToConsoleColor(color);
        Console.Write("     ");
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine($" RGB({color.R,3}, {color.G,3}, {color.B,3})");
    }

    private static ConsoleColor ConvertToConsoleColor(Color color)
    {
        // 简化转换，实际可能需要更复杂的逻辑
        return ConsoleColor.White;
    }
}