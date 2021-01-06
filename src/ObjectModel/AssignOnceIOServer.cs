using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     An io server, which can only be assigned once
    /// </summary>
    public interface IAssignOnceIOServer : IAssignOnce<IIOServer>, IIOServer
    {
    }

    /// <summary>
    ///     An io server, which can only be assigned once. If not assigned, use general IO
    /// </summary>
    public class AssignOnceIOServer : AssignOnce<IIOServer>, IAssignOnceIOServer
    {
        /// <inheritdoc />
        public bool DisableTimeMark
        {
            get => (Element ?? IIOServer.GeneralIO).DisableTimeMark;
            set => (Element ?? IIOServer.GeneralIO).DisableTimeMark = value;
        }

        /// <inheritdoc />
        public bool IsErrorRedirected => (Element ?? IIOServer.GeneralIO).IsErrorRedirected;

        /// <inheritdoc />
        public bool IsOutputRedirected => (Element ?? IIOServer.GeneralIO).IsOutputRedirected;

        /// <inheritdoc />
        public TextWriter ErrorStream
        {
            get => (Element ?? IIOServer.GeneralIO).ErrorStream;
            set => (Element ?? IIOServer.GeneralIO).ErrorStream = value;
        }

        /// <inheritdoc />
        public TextWriter Output
        {
            get => (Element ?? IIOServer.GeneralIO).Output;
            set => (Element ?? IIOServer.GeneralIO).Output = value;
        }

        /// <inheritdoc />
        public string Prefix
        {
            get => (Element ?? IIOServer.GeneralIO).Prefix;
            set => (Element ?? IIOServer.GeneralIO).Prefix = value;
        }

        /// <inheritdoc />
        public IColorSetting ColorSetting
        {
            get => (Element ?? IIOServer.GeneralIO).ColorSetting;
            set => (Element ?? IIOServer.GeneralIO).ColorSetting = value;
        }

        /// <inheritdoc />
        public IPromptServer Prompt
        {
            get => (Element ?? IIOServer.GeneralIO).Prompt;
            set => (Element ?? IIOServer.GeneralIO).Prompt = value;
        }

        /// <inheritdoc />
        public TextReader Input
        {
            get => (Element ?? IIOServer.GeneralIO).Input;
            set => (Element ?? IIOServer.GeneralIO).Input = value;
        }

        /// <inheritdoc />
        public bool IsInputRedirected => (Element ?? IIOServer.GeneralIO).IsInputRedirected;

        /// <inheritdoc />
        public void ResetError()
        {
            (Element ?? IIOServer.GeneralIO).ResetError();
        }

        /// <inheritdoc />
        public void ResetOutput()
        {
            (Element ?? IIOServer.GeneralIO).ResetOutput();
        }

        /// <inheritdoc />
        public void AppendWriteLinePrefix(string str = "\t")
        {
            (Element ?? IIOServer.GeneralIO).AppendWriteLinePrefix(str);
        }

        /// <inheritdoc />
        public void SubtractWriteLinePrefix()
        {
            (Element ?? IIOServer.GeneralIO).SubtractWriteLinePrefix();
        }

        /// <inheritdoc />
        public void Write(string content, ConsoleColor? customColor)
        {
            (Element ?? IIOServer.GeneralIO).Write(content, customColor);
        }

        /// <inheritdoc />
        public void Write(string content, ConsoleColor frontColor, ConsoleColor backColor)
        {
            (Element ?? IIOServer.GeneralIO).Write(content, frontColor, backColor);
        }

        /// <inheritdoc />
        public void Write(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            (Element ?? IIOServer.GeneralIO).Write(content, type, customColor);
        }

        /// <inheritdoc />
        public Task WriteAsync(string content, ConsoleColor frontColor, ConsoleColor backColor)
        {
            return (Element ?? IIOServer.GeneralIO).WriteAsync(content, frontColor, backColor);
        }

        /// <inheritdoc />
        public Task WriteAsync(string content, ConsoleColor? customColor)
        {
            return (Element ?? IIOServer.GeneralIO).WriteAsync(content, customColor);
        }

        /// <inheritdoc />
        public Task WriteAsync(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            return (Element ?? IIOServer.GeneralIO).WriteAsync(content, type, customColor);
        }

        /// <inheritdoc />
        public void WriteLine()
        {
            (Element ?? IIOServer.GeneralIO).WriteLine();
        }

        /// <inheritdoc />
        public void WriteLine(string content, ConsoleColor customColor)
        {
            (Element ?? IIOServer.GeneralIO).WriteLine(content, customColor);
        }

        /// <inheritdoc />
        public void WriteLine(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            (Element ?? IIOServer.GeneralIO).WriteLine(content, type, customColor);
        }

        /// <inheritdoc />
        public void WriteLine(IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
        {
            (Element ?? IIOServer.GeneralIO).WriteLine(contentArray, type);
        }

        /// <inheritdoc />
        public void WriteLine(IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            (Element ?? IIOServer.GeneralIO).WriteLine(contentArray, type);
        }

        /// <inheritdoc />
        public Task WriteLineAsync()
        {
            return (Element ?? IIOServer.GeneralIO).WriteLineAsync();
        }

        /// <inheritdoc />
        public Task WriteLineAsync(string content, ConsoleColor customColor)
        {
            return (Element ?? IIOServer.GeneralIO).WriteLineAsync(content, customColor);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            return (Element ?? IIOServer.GeneralIO).WriteLineAsync(content, type, customColor);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(IEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOServer.GeneralIO).WriteLineAsync(contentArray, type);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(IAsyncEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOServer.GeneralIO).WriteLineAsync(contentArray, type);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOServer.GeneralIO).WriteLineAsync(contentArray, type);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(IAsyncEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOServer.GeneralIO).WriteLineAsync(contentArray, type);
        }

        /// <inheritdoc />
        public void ResetInput()
        {
            (Element ?? IIOServer.GeneralIO).ResetInput();
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt, bool newLine, ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLine(prompt, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLine(prompt, customPromptColor);
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt, string? defaultValue, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLine(prompt, defaultValue, customPromptColor);
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt = null, string? defaultValue = null, bool newLine = false,
            ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLine(prompt, defaultValue, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt, bool newLine, ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLineAsync(prompt, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLineAsync(prompt, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt, string? defaultValue, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLineAsync(prompt, defaultValue, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt = null, string? defaultValue = null, bool newLine = false,
            ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOServer.GeneralIO).ReadLineAsync(prompt, defaultValue, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public int Peek()
        {
            return (Element ?? IIOServer.GeneralIO).Peek();
        }

        /// <inheritdoc />
        public int Read()
        {
            return (Element ?? IIOServer.GeneralIO).Read();
        }

        /// <inheritdoc />
        public string ReadToEnd()
        {
            return (Element ?? IIOServer.GeneralIO).ReadToEnd();
        }

        /// <inheritdoc />
        public Task<string> ReadToEndAsync()
        {
            return (Element ?? IIOServer.GeneralIO).ReadToEndAsync();
        }
    }
}