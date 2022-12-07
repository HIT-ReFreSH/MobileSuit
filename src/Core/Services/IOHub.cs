using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core.Services;

/// <summary>
///     IO hub with pure text output.
/// </summary>
public class PureTextIOHub : IOHub
{
    /// <summary>
    ///     Initialize a IOhub.
    /// </summary>
    public PureTextIOHub(PromptFormatter promptFormatter, IIOHubConfigurator configurator) : base(promptFormatter,
        configurator)
    {
    }

    /// <inheritdoc />
    public override void Write(PrintUnit content)
    {
        Output.Write(content.Text);
    }

    /// <inheritdoc />
    public override async Task WriteAsync(PrintUnit content)
    {
        await Output.WriteAsync(content.Text);
    }
}

/// <summary>
///     IO hub using 4-bit color output.
/// </summary>
public class IOHub4Bit : IOHub
{
    /// <summary>
    ///     Initialize a IOhub.
    /// </summary>
    public IOHub4Bit(PromptFormatter promptFormatter, IIOHubConfigurator configurator) : base(promptFormatter,
        configurator)
    {
    }

    private static int BackgroundCodeOf(Color c)
    {
        return 10 + ForegroundCodeOf(c);
    }

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
            var c = (Color) PrintUnit.ConsoleColorCast(cc)!;
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
        if (content.Foreground is { } || content.Background is { }) Output.Write("\u001b[0m");
    }

    /// <inheritdoc />
    public override async Task WriteAsync(PrintUnit content)
    {
        if (content.Foreground is { } f) await Output.WriteAsync($"\u001b[{ForegroundCodeOf(f)}m");
        if (content.Background is { } b) await Output.WriteAsync($"\u001b[{BackgroundCodeOf(b)}m");
        await Output.WriteAsync(content.Text);
        if (content.Foreground is { } || content.Background is { }) await Output.WriteAsync("\u001b[0m");
    }
}

/// <summary>
///     A entity, which serves the input/output of a mobile suit.
/// </summary>
public class IOHub : IIOHub
{
    /// <summary>
    ///     Initialize a IOServer.
    /// </summary>
    public IOHub(PromptFormatter promptFormatter, IIOHubConfigurator configurator)
    {
        ColorSetting = IColorSetting.DefaultColorSetting;
        Input = Console.In;
        Output = Console.Out;
        ErrorStream = Console.Error;
        FormatPrompt = promptFormatter;
        configurator(this);
    }

    private List<PrintUnit> Prefix { get; } = new();


    /// <inheritdoc />
    public IColorSetting ColorSetting { get; set; }

    /// <inheritdoc />
    public PromptFormatter FormatPrompt { get; }

    /// <inheritdoc />
    public TextReader Input { get; set; }

    /// <inheritdoc />
    public bool IsInputRedirected => !Console.In.Equals(Input);

    /// <inheritdoc />
    public void ResetInput()
    {
        Input = Console.In;
    }

    /// <inheritdoc />
    public string? ReadLine()
    {
        return Input.ReadLine();
    }

    /// <inheritdoc />
    public async Task<string?> ReadLineAsync()
    {
        return await Input.ReadLineAsync();
    }

    /// <inheritdoc />
    public int Peek()
    {
        return Input.Peek();
    }

    /// <inheritdoc />
    public int Read()
    {
        return Input.Read();
    }

    /// <inheritdoc />
    public string ReadToEnd()
    {
        return Input.ReadToEnd();
    }

    /// <inheritdoc />
    public async Task<string> ReadToEndAsync()
    {
        return await Input.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public IOOptions Options { get; set; }

    /// <inheritdoc />
    public bool IsErrorRedirected => !Console.Error.Equals(ErrorStream);

    /// <inheritdoc />
    public bool IsOutputRedirected => !Console.Out.Equals(Output);

    /// <inheritdoc />
    public TextWriter ErrorStream { get; set; }

    /// <inheritdoc />
    public TextWriter Output { get; set; }
    

    /// <inheritdoc />
    public void ResetError()
    {
        ErrorStream = Console.Error;
    }

    /// <inheritdoc />
    public void ResetOutput()
    {
        Output = Console.Out;
    }

    /// <inheritdoc />
    public void AppendWriteLinePrefix(PrintUnit prefix)
    {
        Prefix.Add(prefix);
    }


    /// <inheritdoc />
    public void SubtractWriteLinePrefix()
    {
        Prefix.RemoveAt(Prefix.Count - 1);
    }

    /// <inheritdoc />
    public void ClearWriteLinePrefix()
    {
        Prefix.Clear();
    }

    /// <inheritdoc />
    public virtual void Write(PrintUnit content)
    {
        if (content.Foreground is { } f) Output.Write($"\u001b[38;2;{f.R};{f.G};{f.B}m");
        if (content.Background is { } b) Output.Write($"\u001b[48;2;{b.R};{b.G};{b.B}m");
        Output.Write(content.Text);
        if (content.Foreground is { } || content.Background is { }) Output.Write("\u001b[0m");
    }

    /// <inheritdoc />
    public virtual async Task WriteAsync(PrintUnit content)
    {
        if (content.Foreground is { } f) await Output.WriteAsync($"\u001b[38;2;{f.R};{f.G};{f.B}m");
        if (content.Background is { } b) await Output.WriteAsync($"\u001b[48;2;{b.R};{b.G};{b.B}m");
        await Output.WriteAsync(content.Text);
        if (content.Foreground is { } || content.Background is { }) await Output.WriteAsync("\u001b[0m");
    }


    /// <inheritdoc />
    public IEnumerable<PrintUnit> GetLinePrefix(OutputType type)
    {
        if (!Options.HasFlag(IOOptions.DisableLinePrefix)) return Prefix;
        if (Options.HasFlag(IOOptions.DisableTag)) return Array.Empty<PrintUnit>();
        var sb = new StringBuilder();
        AppendTimeStamp(sb);
        sb.Append(IIOHub.GetLabel(type));
        return new PrintUnit[] {(sb.ToString(), null)};

    }

    private static void AppendTimeStamp(StringBuilder sb)
    {
        sb.Append('[');
        sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        sb.Append(']');
    }
}