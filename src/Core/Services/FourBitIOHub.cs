// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System;
using System.Drawing;
using System.Threading.Tasks;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     IO hub using 4-bit color output.
/// </summary>
public class FourBitIOHub : TrueColorIOHub
{
    /// <summary>
    ///     Initialize a IOhub.
    /// </summary>
    public FourBitIOHub(PromptFormatter promptFormatter, IIOHubConfigurator configurator) : base
    (
        promptFormatter,
        configurator
    ) { }

    private static int BackgroundCodeOf(Color c) { return 10 + ForegroundCodeOf(c); }

    private static int ForegroundCodeOf(Color c)
    {
        return ConsoleColorOf(c) switch
               {
                   ConsoleColor.Black => 30,
                   ConsoleColor.DarkBlue => 34,
                   ConsoleColor.DarkGreen => 32,
                   ConsoleColor.DarkCyan => 36,
                   ConsoleColor.DarkRed => 31,
                   ConsoleColor.DarkMagenta => 35,
                   ConsoleColor.DarkYellow => 33,
                   ConsoleColor.Gray => 90,
                   ConsoleColor.DarkGray => 37,
                   ConsoleColor.Blue => 94,
                   ConsoleColor.Green => 92,
                   ConsoleColor.Cyan => 96,
                   ConsoleColor.Red => 91,
                   ConsoleColor.Magenta => 95,
                   ConsoleColor.Yellow => 93,
                   ConsoleColor.White => 97,
                   _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
               };
    }

    private static ConsoleColor ConsoleColorOf(Color color)
    {
        ConsoleColor re = default;
        double r = color.R, g = color.G, b = color.B, delta = double.MaxValue;

        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
        {
            var c = (Color)PrintUnit.ConsoleColorCast(cc)!;
            var t = Math.Pow(c.R - r, 2.0) + Math.Pow(c.G - g, 2.0) + Math.Pow(c.B - b, 2.0);
            if (t == 0.0)
                return cc;
            if (t >= delta) continue;
            delta = t;
            re = cc;
        }

        return re;
    }

    /// <inheritdoc />
    public override void Write(PrintUnit content)
    {
        if (content.Foreground is { } f) Output.Write($"\u001b[{ForegroundCodeOf(f)}m");
        if (content.Background is { } b) Output.Write($"\u001b[{BackgroundCodeOf(b)}m");
        Output.Write(content.Text);
        if (content.Foreground is not null || content.Background is not null) Output.Write("\u001b[0m");
    }

    /// <inheritdoc />
    public override async Task WriteAsync(PrintUnit content)
    {
        if (content.Foreground is { } f) await Output.WriteAsync($"\u001b[{ForegroundCodeOf(f)}m");
        if (content.Background is { } b) await Output.WriteAsync($"\u001b[{BackgroundCodeOf(b)}m");
        await Output.WriteAsync(content.Text);
        if (content.Foreground is not null || content.Background is not null) await Output.WriteAsync("\u001b[0m");
    }
}