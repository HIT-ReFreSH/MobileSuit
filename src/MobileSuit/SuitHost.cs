#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Status of the last Commandline. Return value type for Built-In-Commands and Host Functions.
    /// </summary>
    public enum TraceBack
    {
        /// <summary>
        ///     The Progress is Exiting
        /// </summary>
        OnExit = 1,

        /// <summary>
        ///     Everything is OK
        /// </summary>
        AllOk = 0,

        /// <summary>
        ///     This is not a command.
        /// </summary>
        InvalidCommand = -1,

        /// <summary>
        ///     Cannot find the object referring to.
        /// </summary>
        ObjectNotFound = -2,

        /// <summary>
        ///     Cannot find the member in the object referring to.
        /// </summary>
        MemberNotFound = -3
    }
    /// <summary>
    /// A entity, which serves the shell functions of a mobile-suit program.
    /// </summary>
    public class SuitHost
    {
        /// <summary>
        /// Initialize a SuitHost with given/general IOServer, given/default BicServer, Calling Assembly.
        /// </summary>
        /// <param name="io">Optional. An IOServer, GeneralIO as default.</param>
        /// <param name="bicServer">Optional. An BicServer, new MobileSuit.SuitBicServer as default.</param>
        public SuitHost(IOServer? io = null, ISuitBuiltInCommandServer? bicServer = null)
        {
            Assembly = Assembly.GetCallingAssembly();
            IO = io ?? GeneralIO;
            Current = new SuitObject(null);
            BicServer = new SuitObject(bicServer ?? new SuitBuiltInCommandServer(this));
        }

        /// <summary>
        /// Initialize a SuitHost with given/general IOServer,  an instance, and its type's Assembly.
        /// </summary>
        /// <param name="instance">The instance for Mobile Suit to drive.</param>
        /// <param name="io">Optional. An IOServer, GeneralIO as default.</param>
        /// <param name="bicServer">Optional. An BicServer, new MobileSuit.SuitBicServer as default.</param>
        public SuitHost(object? instance, IOServer? io = null, ISuitBuiltInCommandServer? bicServer = null) : this(io, bicServer)
        {
            Current = new SuitObject(instance);
            Assembly = WorkType?.Assembly;

            WorkInstanceInit();
        }
        /// <summary>
        /// Initialize a SuitHost with given/general IOServer, given Assembly.
        /// </summary>
        /// <param name="assembly">The given Assembly.</param>
        /// <param name="io">Optional. An IOServer, GeneralIO as default.</param>
        /// <param name="bicServer">Optional. An BicServer, new MobileSuit.SuitBicServer as default.</param>
        public SuitHost(Assembly assembly, IOServer? io = null, ISuitBuiltInCommandServer? bicServer = null) : this(io, bicServer)
        {
            Assembly = assembly;
            Current = new SuitObject(null);
        }
        /// <summary>
        /// Initialize a SuitHost with given/general IOServer,  a given type, and its  Assembly.
        /// </summary>
        /// <param name="type">The given Type</param>
        /// <param name="io">Optional. An IOServer, GeneralIO as default.</param>
        /// <param name="bicServer">Optional. An BicServer, new MobileSuit.SuitBicServer as default.</param>
        public SuitHost(Type type, IOServer? io = null, ISuitBuiltInCommandServer? bicServer = null) : this(io, bicServer)
        {
            if (type?.FullName == null)
            {
                Current = new SuitObject(null);
                return;
            }

            Assembly = type.Assembly;
            Current = new SuitObject(Assembly.CreateInstance(type.FullName));
            WorkInstanceInit();
        }
        /// <summary>
        /// Stack of Instance, created in this Mobile Suit.
        /// </summary>
        public Stack<SuitObject> InstanceStack { get; set; } = new Stack<SuitObject>();
        /// <summary>
        /// String of Current Instance's Name.
        /// </summary>
        public List<string> InstanceNameString { get; set; } = new List<string>();
        /// <summary>
        /// Stack of Instance's Name Strings.
        /// </summary>
        public Stack<List<string>> InstanceNameStringStack { get; set; } = new Stack<List<string>>();
        /// <summary>
        /// If the prompt contains the reference (For example, System.Console.Title) of current instance.
        /// </summary>
        public bool ShowReference { get; set; } = true;
        /// <summary>
        /// The IOServer for this SuitHost
        /// </summary>
        public IOServer IO { get; set; }
        /// <summary>
        /// Default IOServer, using stdin, stdout, stderr.
        /// </summary>
        public static IOServer GeneralIO { get; set; } = new IOServer();
        /// <summary>
        /// The Assembly which instance are from.
        /// </summary>
        public Assembly? Assembly { get; set; }
        /// <summary>
        /// The prompt in Console.
        /// </summary>
        public string? Prompt { get; set; }
        /// <summary>
        /// Current Instance's SuitObject Container.
        /// </summary>
        public SuitObject Current { get; set; }
        /// <summary>
        /// Current BicServer's SuitObject Container.
        /// </summary>
        public SuitObject BicServer { get; set; }
        /// <summary>
        /// Current Instance
        /// </summary>
        public object? WorkInstance => Current.Instance;
        /// <summary>
        /// Current Instance's type.
        /// </summary>
        public Type? WorkType => Current.Instance?.GetType();
        /// <summary>
        /// Use TraceBack, or just throw Exceptions.
        /// </summary>
        public bool UseTraceBack { get; set; } = true;
        /// <summary>
        /// If show that a command has been executed.
        /// </summary>
        public bool ShowDone { get; set; }
        /// <summary>
        /// If this SuitHost runs like a shell that will not exit UNLESS user input exit command.
        /// </summary>
        public bool ShellMode { get; set; } = false;

        private static string[]? SplitCommandLine(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine)) return null;
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
                        if (!string.IsNullOrEmpty(submit))
                            l.Add(submit);
                        left = right + 1;
                        separationPrefix = false;
                        break;
                    default:
                        if (!separating) separationPrefix = true;
                        break;
                }

            submit = commandLine[left..right];
            if (!string.IsNullOrEmpty(submit))
                l.Add(submit);
            return l.ToArray();
        }

        /// <summary>
        /// Initialize the current instance, if it is a SuitClient, or implements IIOInteractive or ICommandInteractive.
        /// </summary>
        public void WorkInstanceInit()
        {
            (WorkInstance as IIOInteractive)?.SetIO(IO);
            (WorkInstance as ICommandInteractive)?.SetCommandHandler(RunCommand);
        }

        private void NotifyAllOk()
        {
            if (UseTraceBack && ShowDone) IO.WriteLine(Lang.Done, OutputType.AllOk);
        }


        private void NotifyError(string errorDescription)
        {
            if (UseTraceBack) IO.WriteLine(errorDescription, OutputType.Error);
            else throw new Exception(errorDescription);
        }

        private void UpdatePrompt(string prompt)
        {
            if (prompt == "" && WorkInstance != null)
            {
                Prompt = (WorkInstance as IInfoProvider)?.Text
                         ?? (WorkType != null
                             ? (WorkType.GetCustomAttribute(typeof(SuitInfoAttribute)) as SuitInfoAttribute
                                ?? new SuitInfoAttribute(WorkInstance.GetType().Name)).Text
                             : prompt);
            }
            else
            {
                Prompt = prompt;
            }

            if (!ShowReference || InstanceNameString.Count <= 0) return;
            var sb = new StringBuilder();
            sb.Append(Prompt);
            sb.Append('[');
            sb.Append(InstanceNameString[0]);
            if (InstanceNameString.Count > 1)
                for (var i = 1; i < InstanceNameString.Count; i++)
                    sb.Append($".{InstanceNameString[i]}");
            sb.Append(']');
            Prompt = sb.ToString();
            if (IO is null) return;
            IO.ConsoleTitle = Prompt;
        }


        private TraceBack RunBuildInCommand(string[] cmdList)
        {
            if (cmdList is null) return TraceBack.InvalidCommand;
            return BicServer.Execute(cmdList, out _);
        }

        private TraceBack RunObject(string[] args)
        {
            var r = Current.Execute(args, out var result);
            if (r == TraceBack.AllOk)
                IO.WriteLine(new (string, ConsoleColor?)[]
                {
                (Lang.ReturnValue, IO.ColorSetting.PromptColor),
                (result.ToString(), null)
                });
            return r;

        }
        /// <summary>
        /// Run a Mobile Suit with Prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns>0, is All ok.</returns>
        public int Run(string prompt)
        {
            UpdatePrompt(prompt);
            for (; ; )
            {
                if (!IO.IsInputRedirected) IO.Write(Prompt + '>', OutputType.Prompt);
                var traceBack = RunCommand(prompt, IO.ReadLine());
                switch (traceBack)
                {
                    case TraceBack.OnExit:
                        return 0;
                    case TraceBack.AllOk:
                        NotifyAllOk();
                        break;
                    case TraceBack.ObjectNotFound:
                        NotifyError(Lang.ObjectNotFound);
                        break;
                    case TraceBack.MemberNotFound:
                        NotifyError(Lang.MemberNotFound);
                        break;
                    default:
                        NotifyError(Lang.InvalidCommand);
                        break;
                }
            }
        }
        /// <summary>
        /// Run some SuitCommands in current environment, until one of them returns a non-AllOK TraceBack.
        /// </summary>
        /// <param name="scripts">SuitCommands</param>
        /// <returns>The TraceBack of the last executed command.</returns>
        public TraceBack RunScripts(IEnumerable<string> scripts)
        {
            foreach (var script in scripts)
            {
                if (script is null) break;
                var traceBack = RunCommand("", script);
                if (traceBack != TraceBack.AllOk) return traceBack;
            }

            return TraceBack.AllOk;
        }
        /// <summary>
        /// Asynchronously run some SuitCommands in current environment, until one of them returns a non-AllOK TraceBack.
        /// </summary>
        /// <param name="scripts">SuitCommands</param>
        /// <returns>The TraceBack of the last executed command.</returns>
        public async Task<TraceBack> RunScriptsAsync(IAsyncEnumerable<string?> scripts)
        {
            await foreach (var script in scripts)
            {
                if (script is null) break;
                var traceBack = RunCommand("", script);
                if (traceBack != TraceBack.AllOk) return traceBack;
            }

            return TraceBack.AllOk;
        }

        private TraceBack RunCommand(string prompt, string? cmd)
        {
            if (string.IsNullOrEmpty(cmd) && IO.IsInputRedirected && ShellMode)
            {
                IO.ResetInput();
                return TraceBack.AllOk;
            }

            if (string.IsNullOrEmpty(cmd)) return TraceBack.AllOk;
            TraceBack traceBack;
            var args = SplitCommandLine(cmd);
            if (args is null) return TraceBack.InvalidCommand;
            try
            {
                if (cmd[0] == '@')
                {
                    args[0] = args[0][1..];
                    traceBack = RunBuildInCommand(args);
                }

                traceBack = RunObject(args);
                if (traceBack == TraceBack.ObjectNotFound) traceBack = RunBuildInCommand(args);
            }
            catch (Exception e)
            {
                IO.Error.WriteLine(e.ToString());
                traceBack = TraceBack.InvalidCommand;
            }

            UpdatePrompt(prompt);
            return traceBack;
        }
        /// <summary>
        /// Run a Mobile Suit with default Prompt.
        /// </summary>
        /// <returns>0, is All ok.</returns>
        public int Run()
        {
            return Run("");
        }
    }
}