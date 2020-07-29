using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// A host of Mobile Suit, which may run commands.
    /// </summary>
    public interface IMobileSuitHost
    {
        /// <summary>
        /// Split a commandline string to args[] array.
        /// </summary>
        /// <param name="commandLine">commandline string</param>
        /// <returns>args[] array</returns>
        protected static string[]? SplitCommandLine(string commandLine)
        {
            if (String.IsNullOrEmpty(commandLine)) return null;
            string submit;
            var l = new List<string>();
            var separating = false;
            var separationPrefix = false;
            var separationCharacter = '"';
            var left = 0;
            var right = 0;
            for (; right < commandLine.Length; right++)
                switch (commandLine[right])
                {
                    case '"':
                        if (separationPrefix) continue;
                        if (separating && separationCharacter == '"')
                        {
                            l.Add(commandLine[left..right]);
                            left = right + 1;
                        }
                        else if (!separating)
                        {
                            separating = true;
                            separationCharacter = '"';
                            left = right + 1;
                        }

                        break;
                    case '\'':
                        if (separationPrefix) continue;
                        if (separating && separationCharacter == '\'')
                        {
                            l.Add(commandLine[left..right]);
                            left = right + 1;
                        }
                        else if (!separating)
                        {
                            separating = true;
                            separationCharacter = '\'';
                            left = right + 1;
                        }

                        break;
                    case ' ':
                        submit = commandLine[left..right];
                        if (!String.IsNullOrEmpty(submit))
                            l.Add(submit);
                        left = right + 1;
                        separationPrefix = false;
                        break;
                    default:
                        if (!separating) separationPrefix = true;
                        break;
                }

            submit = commandLine[left..right];
            if (!String.IsNullOrEmpty(submit))
                l.Add(submit);
            return l.ToArray();
        }
        /// <summary>
        /// Basic Settings of this MobileSuitHost
        /// </summary>
        HostSettings Settings { get; set; }

        /// <summary>
        ///     Run a Mobile Suit with Prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns>0, is All ok.</returns>
        int Run(string prompt);

        /// <summary>
        ///     Run a Mobile Suit with default Prompt.
        /// </summary>
        /// <returns>0, is All ok.</returns>
        public int Run();

        /// <summary>
        ///     Asynchronously run some SuitCommands in current environment, until one of them returns a non-AllOK TraceBack.
        /// </summary>
        /// <param name="scripts">SuitCommands</param>
        /// <param name="withPrompt">if this run contains prompt, or silent</param>
        /// <param name="scriptName">name of these scripts</param>
        /// <returns>The TraceBack of the last executed command.</returns>
        Task<TraceBack> RunScriptsAsync(IAsyncEnumerable<string?> scripts, bool withPrompt = false,
            string? scriptName = null);

        /// <summary>
        ///     Run some SuitCommands in current environment, until one of them returns a non-AllOK TraceBack.
        /// </summary>
        /// <param name="scripts">SuitCommands</param>
        /// <param name="withPrompt">if this run contains prompt, or silent</param>
        /// <param name="scriptName">name of these scripts</param>
        /// <returns>The TraceBack of the last executed command.</returns>
        TraceBack RunScripts(IEnumerable<string> scripts, bool withPrompt = false, string? scriptName = null);

        /// <summary>
        /// Run a command in current host with given prompt
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="prompt">the prompt</param>
        /// <returns>result of the command</returns>
        public TraceBack RunCommand( string? command,string prompt="");

    }

    /// <summary>
    /// Basic Settings of a MobileSuitHost
    /// </summary>
    public struct HostSettings : IEquatable<HostSettings>
    {

        /// <summary>
        ///     Throw Exceptions, instead of using TraceBack.
        /// </summary>
        public bool EnableThrows { get; set; }

        /// <summary>
        ///     If show that a command has been executed.
        /// </summary>
        public bool ShowDone { get; set; }

        /// <summary>
        ///     If this SuitHost will not exit UNLESS user input exit command.
        /// </summary>
        public bool NoExit { get; set; }

        /// <summary>
        ///     whether mobile Suit shows command return value or not.
        /// </summary>
        public bool HideReturnValue { get; set; }

        /// <summary>
        /// Compares two HostSettings 
        /// </summary>
        /// <param name="other">Another HostSettings</param>
        /// <returns></returns>
        public bool Equals(HostSettings other)
        {
            return EnableThrows == other.EnableThrows
                   && ShowDone == other.ShowDone
                   && NoExit == other.NoExit
                   && HideReturnValue == other.HideReturnValue;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is HostSettings other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(EnableThrows, ShowDone, NoExit, HideReturnValue);
        }

        /// <summary>
        /// Compares two HostSettings 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(HostSettings a, HostSettings b)
            => a.Equals(b);

        /// <summary>
        /// Compares two HostSettings 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(HostSettings a, HostSettings b)
        {
            return !(a == b);
        }
    }
}
