using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.UI;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     An io server, which can only be assigned once
    /// </summary>
    public interface IAssignOnceIOHub : IAssignOnce<IIOHub>, IIOHub
    {
    }

    /// <summary>
    ///     An io server, which can only be assigned once. If not assigned, use general IO
    /// </summary>
    public class AssignOnceIOHub : AssignOnce<IIOHub>, IAssignOnceIOHub
    {
        public IInputHelper InputHelper { get; }

        /// <inheritdoc />
        public bool DisableTimeMark
        {
            get => (Element ?? IIOHub.GeneralIO).DisableTimeMark;
            set => (Element ?? IIOHub.GeneralIO).DisableTimeMark = value;
        }

        /// <inheritdoc />
        public bool IsErrorRedirected => (Element ?? IIOHub.GeneralIO).IsErrorRedirected;

        /// <inheritdoc />
        public bool IsOutputRedirected => (Element ?? IIOHub.GeneralIO).IsOutputRedirected;

        /// <inheritdoc />
        public TextWriter ErrorStream
        {
            get => (Element ?? IIOHub.GeneralIO).ErrorStream;
            set => (Element ?? IIOHub.GeneralIO).ErrorStream = value;
        }

        /// <inheritdoc />
        public TextWriter Output
        {
            get => (Element ?? IIOHub.GeneralIO).Output;
            set => (Element ?? IIOHub.GeneralIO).Output = value;
        }

        /// <inheritdoc />
        public string Prefix
        {
            get => (Element ?? IIOHub.GeneralIO).Prefix;
            set => (Element ?? IIOHub.GeneralIO).Prefix = value;
        }

        /// <inheritdoc />
        public IColorSetting ColorSetting
        {
            get => (Element ?? IIOHub.GeneralIO).ColorSetting;
            set => (Element ?? IIOHub.GeneralIO).ColorSetting = value;
        }

        /// <inheritdoc />
        public PromptGenerator Prompt
        {
            get => (Element ?? IIOHub.GeneralIO).Prompt;
            set => (Element ?? IIOHub.GeneralIO).Prompt = value;
        }

        /// <inheritdoc />
        public TextReader Input
        {
            get => (Element ?? IIOHub.GeneralIO).Input;
            set => (Element ?? IIOHub.GeneralIO).Input = value;
        }

        /// <inheritdoc />
        public bool IsInputRedirected => (Element ?? IIOHub.GeneralIO).IsInputRedirected;

        /// <inheritdoc />
        public void ResetError()
        {
            (Element ?? IIOHub.GeneralIO).ResetError();
        }

        /// <inheritdoc />
        public void ResetOutput()
        {
            (Element ?? IIOHub.GeneralIO).ResetOutput();
        }

        /// <inheritdoc />
        public void AppendWriteLinePrefix(string str = "\t")
        {
            (Element ?? IIOHub.GeneralIO).AppendWriteLinePrefix(str);
        }

        /// <inheritdoc />
        public void SubtractWriteLinePrefix()
        {
            (Element ?? IIOHub.GeneralIO).SubtractWriteLinePrefix();
        }

        /// <inheritdoc />
        public void Write(string content, ConsoleColor? customColor)
        {
            (Element ?? IIOHub.GeneralIO).Write(content, customColor);
        }

        /// <inheritdoc />
        public void Write(string content, ConsoleColor frontColor, ConsoleColor backColor)
        {
            (Element ?? IIOHub.GeneralIO).Write(content, frontColor, backColor);
        }

        /// <inheritdoc />
        public void Write(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            (Element ?? IIOHub.GeneralIO).Write(content, type, customColor);
        }

        /// <inheritdoc />
        public Task WriteAsync(string content, ConsoleColor frontColor, ConsoleColor backColor)
        {
            return (Element ?? IIOHub.GeneralIO).WriteAsync(content, frontColor, backColor);
        }

        /// <inheritdoc />
        public Task WriteAsync(string content, ConsoleColor? customColor)
        {
            return (Element ?? IIOHub.GeneralIO).WriteAsync(content, customColor);
        }

        /// <inheritdoc />
        public Task WriteAsync(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            return (Element ?? IIOHub.GeneralIO).WriteAsync(content, type, customColor);
        }

        /// <inheritdoc />
        public void WriteLine()
        {
            (Element ?? IIOHub.GeneralIO).WriteLine();
        }

        /// <inheritdoc />
        public void WriteLine(string content, ConsoleColor customColor)
        {
            (Element ?? IIOHub.GeneralIO).WriteLine(content, customColor);
        }

        /// <inheritdoc />
        public void WriteLine(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            (Element ?? IIOHub.GeneralIO).WriteLine(content, type, customColor);
        }

        public void Write(IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void WriteLine(IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
        {
            (Element ?? IIOHub.GeneralIO).WriteLine(contentArray, type);
        }

        /// <inheritdoc />
        public void WriteLine(IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            (Element ?? IIOHub.GeneralIO).WriteLine(contentArray, type);
        }

        /// <inheritdoc />
        public Task WriteLineAsync()
        {
            return (Element ?? IIOHub.GeneralIO).WriteLineAsync();
        }

        /// <inheritdoc />
        public Task WriteLineAsync(string content, ConsoleColor customColor)
        {
            return (Element ?? IIOHub.GeneralIO).WriteLineAsync(content, customColor);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            return (Element ?? IIOHub.GeneralIO).WriteLineAsync(content, type, customColor);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(IEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOHub.GeneralIO).WriteLineAsync(contentArray, type);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(IAsyncEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOHub.GeneralIO).WriteLineAsync(contentArray, type);
        }

        /// <inheritdoc />
        public Task WriteLineAsync(IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOHub.GeneralIO).WriteLineAsync(contentArray, type);
        }
        /// <inheritdoc />
        public Task WriteAsync(IAsyncEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
            => Element?.WriteAsync(contentArray, type) ?? Task.CompletedTask;
        /// <inheritdoc />
        public Task WriteAsync(IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
            => Element?.WriteAsync(contentArray, type) ?? Task.CompletedTask;
        /// <inheritdoc />
        public Task WriteAsync(IAsyncEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
            => Element?.WriteAsync(contentArray, type) ?? Task.CompletedTask;

        /// <inheritdoc />
        public Task WriteAsync(IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
            => Element?.WriteAsync(contentArray, type) ?? Task.CompletedTask;

        /// <inheritdoc />
        public Task WriteLineAsync(IAsyncEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            return (Element ?? IIOHub.GeneralIO).WriteLineAsync(contentArray, type);
        }

        /// <inheritdoc />
        public void ResetInput()
        {
            (Element ?? IIOHub.GeneralIO).ResetInput();
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt, bool newLine, ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLine(prompt, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLine(prompt, customPromptColor);
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt, string? defaultValue, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLine(prompt, defaultValue, customPromptColor);
        }

        /// <inheritdoc />
        public string? ReadLine(string? prompt = null, string? defaultValue = null, bool newLine = false,
            ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLine(prompt, defaultValue, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt, bool newLine, ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLineAsync(prompt, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLineAsync(prompt, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt, string? defaultValue, ConsoleColor? customPromptColor)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLineAsync(prompt, defaultValue, customPromptColor);
        }

        /// <inheritdoc />
        public Task<string?> ReadLineAsync(string? prompt = null, string? defaultValue = null, bool newLine = false,
            ConsoleColor? customPromptColor = null)
        {
            return (Element ?? IIOHub.GeneralIO).ReadLineAsync(prompt, defaultValue, newLine, customPromptColor);
        }

        /// <inheritdoc />
        public int Peek()
        {
            return (Element ?? IIOHub.GeneralIO).Peek();
        }

        /// <inheritdoc />
        public int Read()
        {
            return (Element ?? IIOHub.GeneralIO).Read();
        }

        /// <inheritdoc />
        public string ReadToEnd()
        {
            return (Element ?? IIOHub.GeneralIO).ReadToEnd();
        }

        /// <inheritdoc />
        public Task<string> ReadToEndAsync()
        {
            return (Element ?? IIOHub.GeneralIO).ReadToEndAsync();
        }
    }
}