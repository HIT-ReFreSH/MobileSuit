// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HitRefresh.MobileSuit.Core.Services;

namespace HitRefresh.MobileSuit.Core;

/// <summary>
///     Well, <see cref="IIOHub">IIOHub</see> is just a alias for this `IIOHubYouShouldNeverUse`.
///     We designed this to help you use the task log function correctly in any IIOHub adapter.
///     If you don't understand why this is the case, please just use IIOHub.
///     Yes, you should NEVER use this interface directly, unless you are me, haha
/// </summary>
public interface IIOHubYouShouldNeverUse
{
    /*/// <summary>
    /// A input helper for current IOHub. Provides information to the prompt.
    /// </summary>
    public IInputHelper InputHelper { get; }*/
    /// <summary>
    ///     Disable Time marks which shows in Output-Redirected Environment.
    /// </summary>
    public IOOptions Options { get; set; }


    /// <summary>
    ///     Check if this IOServer's error stream is redirected (NOT stderr)
    /// </summary>
    bool IsErrorRedirected { get; }

    /// <summary>
    ///     Check if this IOServer's output stream is redirected (NOT stdout)
    /// </summary>
    bool IsOutputRedirected { get; }

    /// <summary>
    ///     Error stream (default stderr)
    /// </summary>
    TextWriter ErrorStream { get; set; }

    /// <summary>
    ///     Output stream (default stdout)
    /// </summary>
    TextWriter Output { get; set; }


    /// <summary>
    ///     Color settings for this IOServer. (default DefaultColorSetting)
    /// </summary>
    IColorSetting ColorSetting { get; set; }

    /// <summary>
    ///     Input stream (default stdin)
    /// </summary>
    TextReader Input { get; set; }

    /// <summary>
    ///     Checks if this IOServer's input stream is redirected (NOT stdin)
    /// </summary>
    bool IsInputRedirected { get; }

    /// <summary>
    ///     Prompt server for the io server.
    /// </summary>
    public PromptFormatter FormatPrompt { get; }


    /// <summary>
    ///     get label of given output type
    /// </summary>
    /// <param name="type">output type</param>
    /// <returns>label</returns>
    protected static string GetLabel(OutputType type = OutputType.Default)
    {
        return type switch
               {
                   OutputType.Default => "",
                   OutputType.Prompt => "[Prompt]",
                   OutputType.Error => "[Error]",
                   OutputType.Ok => "[AllOk]",
                   OutputType.Title => "[List]",
                   OutputType.Info => "[Info]",
                   OutputType.System => "[System]",
                   _ => ""
               };
    }


    /// <summary>
    ///     Reset this IOServer's error stream to stderr
    /// </summary>
    void ResetError();

    /// <summary>
    ///     Reset this IOServer's output stream to stdout
    /// </summary>
    void ResetOutput();

    /// <summary>
    ///     Append a str to Prefix, usually used to increase indentation
    /// </summary>
    /// <param name="prefix">the output tuple to append</param>
    void AppendWriteLinePrefix(PrintUnit prefix);

    /// <summary>
    ///     Subtract a str from Prefix, usually used to decrease indentation
    /// </summary>
    void SubtractWriteLinePrefix();

    /// <summary>
    ///     Clear the prefix before writing line.
    /// </summary>
    void ClearWriteLinePrefix();

    /// <summary>
    ///     Writes some content to output stream, with line break. With certain Input/Output color.
    /// </summary>
    /// <param name="content">
    ///     Output Tuple
    ///     first is a part of content;
    ///     second is optional, the foreground color of output (in console),
    ///     third is optional, the background color of output.
    /// </param>
    void Write(PrintUnit content);

    /// <summary>
    ///     Writes some content to output stream, with line break. With certain Input/Output color.
    /// </summary>
    /// <param name="content">
    ///     Output Tuple
    ///     first is a part of content;
    ///     second is optional, the foreground color of output (in console),
    ///     third is optional, the background color of output.
    /// </param>
    Task WriteAsync(PrintUnit content);

    /// <summary>
    ///     Get the prefix before writing line.
    /// </summary>
    /// <returns></returns>
    IEnumerable<PrintUnit> GetLinePrefix(OutputType type);

    /// <summary>
    ///     Reset this IOServer's input stream to stdin
    /// </summary>
    void ResetInput();

    /// <summary>
    ///     Reads a line from input stream, with prompt.
    /// </summary>
    /// <returns>Content from input stream, null if EOF</returns>
    string? ReadLine();

    /// <summary>
    ///     Reads a line from input stream, with prompt.
    /// </summary>
    /// <returns>Content from input stream, null if EOF</returns>
    Task<string?> ReadLineAsync();

    /// <summary>
    ///     Reads the next character from input stream without changing the state of the reader or the character source.
    /// </summary>
    /// <returns>The next available character.</returns>
    int Peek();

    /// <summary>
    ///     Reads the next character from input stream.
    /// </summary>
    /// <returns>The next available character.</returns>
    int Read();

    /// <summary>
    ///     Reads all characters from the current position to the end of the input stream and returns them as one string.
    /// </summary>
    /// <returns>All characters from the current position to the end.</returns>
    string ReadToEnd();

    /// <summary>
    ///     Asynchronously reads all characters from the current position to the end of the input stream and returns them as
    ///     one string.
    /// </summary>
    /// <returns>All characters from the current position to the end.</returns>
    Task<string> ReadToEndAsync();
}