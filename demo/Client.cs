using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HitRefresh.MobileSuit;
using HitRefresh.MobileSuit.UI;
using HitRefresh.MobileSuit.Core;
using HitRefresh.MobileSuit.Themes;
using System.Drawing;
namespace HitRefresh.MobileSuitDemo;

[SuitInfo("Demo")]
public class Client
{
    /// <summary>
    ///     Initialize a client
    /// </summary>
    public Client(IIOHub io)
    {
        IO = io;
    }

    public IIOHub IO { get; }

    public void Loop([SuitInjected] CancellationToken token)
    {
        for (;;)
            if (token.IsCancellationRequested)
                return;
    }

    [SuitAlias("H")]
    [SuitInfo("hello command.")]
    public void Hello()
    {
        IO.WriteLine("Hello! MobileSuit!");
    }

    [SuitAlias("Sl")]
    [SuitInfo("Sleep {-n name (, -t hours, -s)}")]
    public void Sleep(SleepArgument argument)
    {
        var nameChain = "";
        foreach (var item in argument.Name) nameChain += item;
        if (nameChain == "") nameChain = "No one";

        if (argument.IsSleeping)
            IO.WriteLine(nameChain + " has been sleeping for " + argument.SleepTime + " hour(s).");
        else
            IO.WriteLine(nameChain + " is not sleeping.");
    }

    [SuitAlias("cui")]
    public void CuiTest()
    {
        var selected=IO.CuiSelectItemFrom("Select one", new[] { "x", "y", "z" }, null,
            (_, x) => x);
        var yes = IO.CuiYesNo($"You've selected {selected}, aren't you? SelectMany?");
        if (yes)
        {
            var selecteds = IO.CuiSelectItemsFrom("Select many", new[]
                {
                    "w","x", "y", "z"
                }, null,
                (_, x) => x);
            Console.WriteLine($@"{string.Join(",",selecteds)} is selected");
        }
    }

    public static object NumberConvert(string arg)
    {
        return int.Parse(arg);
    }

    [SuitAlias("Sn")]
    public void ShowNumber(int i)
    {
        IO.WriteLine(i.ToString());
    }

    [SuitAlias("Sn2")]
    public void ShowNumber2(int i, int[] j)
    {
        IO.WriteLine(i.ToString());
        IO.WriteLine(j.Length >= 1 ? j[0].ToString() : "");
    }

    [SuitAlias("GE")]
    public void GoodEvening(string[] arg)
    {
        IO.WriteLine("Good Evening, " + (arg.Length >= 1 ? arg[0] : ""));
    }

    [SuitAlias("GE2")]
    public void GoodEvening2(string arg0, string[] args)
    {
        IO.WriteLine("Good Evening, " + arg0 + (args.Length >= 1 ? " and " + args[0] : ""));
    }

    [SuitAlias("GM")]
    public void GoodMorning(GoodMorningParameter arg)
    {
        IO.WriteLine("Good morning," + arg.name);
    }

    [SuitAlias("GM2")]
    public void GoodMorning2(string arg, GoodMorningParameter arg1)
    {
        IO.WriteLine("Good morning, " + arg + " and " + arg1.name);
    }

    public string Bye(string name)
    {
        IO.WriteLine($"Bye! {name}");
        return "bye";
    }

    public string Bye()
    {
        return $"bye, {IO.ReadLine("Name", "foo", true)}";
    }

    public async Task<string> HelloAsync()
    {
        await Task.Delay(10);
        return "Hello from async Task<string>!";
    }

    public async Task<string> HelloAsync(string name)
    {
        await Task.Delay(10);
        return $"Hello, {name}, from async Task<string>!";
    }

[SuitAlias("theme")]
[SuitInfo("Switch color theme (nord, dracula, solarized-light, solarized-dark, monokai, default)")]
public void SwitchTheme(string themeName)
{
    try
    {
        IColorSetting theme = themeName.ToLower() switch
        {
            "nord" => new NordTheme(),
            "dracula" => new DraculaTheme(),
            "solarized-light" => new SolarizedLightTheme(),
            "solarized-dark" => new SolarizedDarkTheme(),
            "monokai" => new MonokaiTheme(),
            "default" => IColorSetting.DefaultColorSetting,
            _ => throw new ArgumentException($"Unknown theme: {themeName}")
        };

        IO.ColorSetting = theme;
        IO.WriteLine($" Switched to {themeName} theme", IO.ColorSetting.OkColor);

        // 立即显示该主题的特征
        ShowThemeCharacteristics(themeName, theme);
    }
    catch (Exception ex)
    {
        IO.WriteLine($" Failed to switch theme: {ex.Message}", IO.ColorSetting.ErrorColor);
    }
}

private void ShowThemeCharacteristics(string themeName, IColorSetting theme)
{
    // 显示主题特征
    IO.WriteLine("\nTHEME CHARACTERISTICS:", theme.TitleColor);
    IO.WriteLine(new string('─', 50), theme.SystemColor);

    // 显示颜色角色
    ShowColorRole("Default text", theme.DefaultColor);
    ShowColorRole("Command prompt", theme.PromptColor);
    ShowColorRole("Success/OK", theme.OkColor);
    ShowColorRole("Error", theme.ErrorColor);
    ShowColorRole("Information", theme.InformationColor);
    ShowColorRole("Warning", theme.WarningColor);
    ShowColorRole("Title/Header", theme.TitleColor);

    // 显示主题特色描述
    IO.WriteLine("\nTHEME STYLE:", theme.TitleColor);

    // 根据主题显示不同的描述
    string description = themeName.ToLower() switch
    {
        "nord" => "Arctic, north-bluish color palette. Focus on clear, clean design.",
        "dracula" => "Dark theme with vibrant, contrasting colors. High visibility.",
        "solarized-light" => "Precision colors for eye comfort. Subtle, low-contrast.",
        "solarized-dark" => "Dark variant of Solarized. Balanced, eye-friendly.",
        "monokai" => "Vibrant, high-contrast colors based on Sublime Text.",
        "default" => "Standard console colors. Simple and functional.",
        _ => "Custom theme."
    };

    IO.WriteLine($"  {description}", theme.InformationColor);

    // 显示亮度信息
    double brightness = CalculateBrightness(theme.DefaultColor);
    double contrast = CalculateContrast(theme.DefaultColor, theme.BackgroundColor);

    IO.WriteLine("\nCOLOR METRICS:", theme.TitleColor);
    IO.WriteLine($"  Text brightness: {brightness:F0}%",
        brightness > 60 ? theme.OkColor : brightness < 30 ? theme.WarningColor : theme.InformationColor);
    IO.WriteLine($"  Contrast ratio: {contrast:F1}:1",
        contrast > 4.5 ? theme.OkColor : theme.WarningColor);

    // 给出建议
    IO.WriteLine("\n TIPS:", theme.TitleColor);
    if (contrast > 7)
        IO.WriteLine("  Excellent readability (WCAG AAA)", theme.OkColor);
    else if (contrast > 4.5)
        IO.WriteLine("  Good readability (WCAG AA)", theme.OkColor);
    else
        IO.WriteLine("  Consider higher contrast for better readability", theme.WarningColor);

    if (themeName.ToLower() == "solarized-light" || themeName.ToLower() == "solarized-dark")
        IO.WriteLine("  Designed for long coding sessions", theme.InformationColor);

    IO.WriteLine(new string('─', 50), theme.SystemColor);
}

private void ShowColorRole(string roleName, Color color)
{
    IO.WriteLine($"  {roleName,-15}: ", IO.ColorSetting.DefaultColor);

    // 显示颜色块
    IO.WriteLine("███ ", color);

    // 显示RGB值
    IO.WriteLine($"RGB({color.R,3},{color.G,3},{color.B,3})", IO.ColorSetting.InformationColor);
}

private double CalculateBrightness(Color color)
{
    // 计算颜色亮度 (0-100%)
    return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 2.55;
}

private double CalculateContrast(Color foreground, Color background)
{
    // 简单的对比度计算
    double fgLuminance = (0.299 * foreground.R + 0.587 * foreground.G + 0.114 * foreground.B) / 255;
    double bgLuminance = (0.299 * background.R + 0.587 * background.G + 0.114 * background.B) / 255;

    double lighter = Math.Max(fgLuminance, bgLuminance);
    double darker = Math.Min(fgLuminance, bgLuminance);

    return (lighter + 0.05) / (darker + 0.05);
}




    [SuitAlias("themes")]
    [SuitInfo("List all available themes")]
    public void ListThemes()
    {
        var themes = new[]
        {
            "nord", "dracula", "solarized-light", "solarized-dark", "monokai", "default"
        };

        IO.WriteLine("Available themes:", IO.ColorSetting.TitleColor);
        foreach (var theme in themes)
        {
            IO.WriteLine($"  {theme}", IO.ColorSetting.InformationColor);
        }
    }


    private void ShowColorValue(Color color)
    {
        // 显示颜色的 RGB 值
        IO.WriteLine($" {color.R,3},{color.G,3},{color.B,3} ", color);
    }
    public class SleepArgument : AutoDynamicParameter
    {
        [Option("n"),AsCollection,WithDefault]
        public List<string> Name { get; set; } = new();

        [Option("t"),WithDefault] public int SleepTime { get; set; } = 0;

        [Switch("s")] public bool IsSleeping { get; set; }
    }

    public class GoodMorningParameter : IDynamicParameter
    {
        public string name = "foo";

        /**
             * Parse this Parameter from String[].
             *
             * @param options String[] to parse from.
             * @return Whether the parsing is successful
             */
        public bool Parse(IReadOnlyList<string> args, SuitContext context)
        {
            if (args.Count == 1)
            {
                name = args[0];
                return true;
            }

            return args.Count == 0;
        }
    }
}