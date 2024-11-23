// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     This is the real IIOHub wherever else you're using. This is used for Background Task Logging.
/// </summary>
public class RealIOHub
(
    ISuitLogBucket bucket,
    ISuitContextProperties properties,
    IIOHubYouShouldNeverUse delegation
) : IIOHub
{
    /// <summary>
    ///     Specifies whether this RealIOHub is for background task or a foreground request.
    /// </summary>
    protected bool IsTask => properties.ContainsKey(SuitBuildUtils.SUIT_TASK_FLAG);


    /// <inheritdoc />
    public IOOptions Options { get => delegation.Options; set => delegation.Options = value; }

    /// <inheritdoc />
    public bool IsErrorRedirected => delegation.IsErrorRedirected;

    /// <inheritdoc />
    public bool IsOutputRedirected => delegation.IsOutputRedirected;

    /// <inheritdoc />
    public TextWriter ErrorStream { get => delegation.ErrorStream; set => delegation.ErrorStream = value; }

    /// <inheritdoc />
    public TextWriter Output { get => delegation.Output; set => delegation.Output = value; }

    /// <inheritdoc />
    public IColorSetting ColorSetting { get => delegation.ColorSetting; set => delegation.ColorSetting = value; }

    /// <inheritdoc />
    public TextReader Input { get => delegation.Input; set => delegation.Input = value; }

    /// <inheritdoc />
    public bool IsInputRedirected => delegation.IsInputRedirected;

    /// <inheritdoc />
    public PromptFormatter FormatPrompt => delegation.FormatPrompt;

    /// <inheritdoc />
    public void ResetError() { delegation.ResetError(); }

    /// <inheritdoc />
    public void ResetOutput() { delegation.ResetOutput(); }

    /// <inheritdoc />
    public void AppendWriteLinePrefix(PrintUnit prefix) { delegation.AppendWriteLinePrefix(prefix); }

    /// <inheritdoc />
    public void SubtractWriteLinePrefix() { delegation.SubtractWriteLinePrefix(); }

    /// <inheritdoc />
    public void ClearWriteLinePrefix() { delegation.ClearWriteLinePrefix(); }

    /// <inheritdoc />
    public void Write(PrintUnit content)
    {
        (IsTask
             ? (Action<PrintUnit>)bucket.Add
             : delegation.Write)(content);
    }

    /// <inheritdoc />
    public Task WriteAsync(PrintUnit content)
    {
        if (!IsTask) return delegation.WriteAsync(content);
        bucket.Add(content);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public IEnumerable<PrintUnit> GetLinePrefix(OutputType type) { return delegation.GetLinePrefix(type); }

    /// <inheritdoc />
    public void ResetInput() { delegation.ResetInput(); }

    /// <inheritdoc />
    public string? ReadLine() { return delegation.ReadLine(); }

    /// <inheritdoc />
    public Task<string?> ReadLineAsync() { return delegation.ReadLineAsync(); }

    /// <inheritdoc />
    public int Peek() { return delegation.Peek(); }

    /// <inheritdoc />
    public int Read() { return delegation.Read(); }

    /// <inheritdoc />
    public string ReadToEnd() { return delegation.ReadToEnd(); }

    /// <inheritdoc />
    public Task<string> ReadToEndAsync() { return delegation.ReadToEndAsync(); }
}