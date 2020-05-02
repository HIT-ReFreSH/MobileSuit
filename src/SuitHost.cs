#nullable enable

using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace PlasticMetal.MobileSuit
{

    /// <summary>
    /// A entity, which serves the shell functions of a mobile-suit program.
    /// </summary>
    public class SuitHost
    {


        /// <summary>
        /// Initialize a SuitHost with given configuration, Calling Assembly.
        /// </summary>
        /// <param name="configuration">given configuration</param>
        public SuitHost(ISuitConfiguration configuration)
        {
            _returnValue = "";
            Configuration = configuration ?? ISuitConfiguration.GetDefaultConfiguration();
            Assembly = Assembly.GetCallingAssembly();
            Current = new SuitObject(null);
            Configuration.InitializeBuildInCommandServer(this);
            BicServer = new SuitObject(Configuration.BuildInCommandServer);
            IO.ColorSetting = Configuration.ColorSetting;
            IO.Prompt = Prompt;
        }

        /// <summary>
        /// Initialize a SuitHost with given/default configuration,  an instance, and its type's Assembly.
        /// </summary>
        /// <param name="instance">The instance for Mobile Suit to drive.</param>
        /// <param name="configuration">given configuration</param>
        public SuitHost(object? instance, ISuitConfiguration? configuration = null) : this(configuration ?? ISuitConfiguration.GetDefaultConfiguration())
        {
            Current = new SuitObject(instance);
            Assembly = WorkType?.Assembly;

            WorkInstanceInit();
        }
        /// <summary>
        /// Initialize a SuitHost with given/default configuration, given Assembly.
        /// </summary>
        /// <param name="assembly">The given Assembly.</param>
        /// <param name="configuration">given configuration, default if null</param>
        public SuitHost(Assembly assembly, ISuitConfiguration? configuration) : this(configuration ?? ISuitConfiguration.GetDefaultConfiguration())
        {
            Assembly = assembly;
            Current = new SuitObject(null);
        }
        /// <summary>
        /// Initialize a SuitHost with given configuration,  a given type, and its  Assembly.
        /// </summary>
        /// <param name="type">The given Type</param>
        /// <param name="configuration">given configuration, default if null</param>
        public SuitHost(Type type, ISuitConfiguration? configuration) : this(configuration ?? ISuitConfiguration.GetDefaultConfiguration())
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
        public Stack<SuitObject> InstanceStack { get; } = new Stack<SuitObject>();
        /// <summary>
        /// String of Current Instance's Name.
        /// </summary>
        public List<string> InstanceNameString { get; } = new List<string>();
        /// <summary>
        /// Stack of Instance's Name Strings.
        /// </summary>
        public Stack<List<string>> InstanceNameStringStack { get; } = new Stack<List<string>>();
        /// <summary>
        /// If the prompt contains the reference (For example, System.Console.Title) of current instance.
        /// </summary>
        public bool ShowReference { get; set; } = true;

        /// <summary>
        /// The IOServer for this SuitHost
        /// </summary>
        public IIOServer IO => Configuration.IO;
        /// <summary>
        /// The configuration used to initialize the mobile suit
        /// </summary>
        public ISuitConfiguration Configuration { get; }

        /// <summary>
        /// The Assembly which instance are from.
        /// </summary>
        public Assembly? Assembly { get; set; }

        /// <summary>
        /// The prompt in Console.
        /// </summary>
        public IPromptServer Prompt => Configuration.PromptServer;
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
            if (IsNullOrEmpty(commandLine)) return null;
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
                        if (!IsNullOrEmpty(submit))
                            l.Add(submit);
                        left = right + 1;
                        separationPrefix = false;
                        break;
                    default:
                        if (!separating) separationPrefix = true;
                        break;
                }

            submit = commandLine[left..right];
            if (!IsNullOrEmpty(submit))
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
            if (UseTraceBack) IO.WriteLine(errorDescription + '!', OutputType.Error);
            else throw new Exception(errorDescription);
        }

        private string UpdatePrompt(string prompt)
        {
            if (IsNullOrEmpty(prompt) && WorkInstance != null)
            {
                return WorkType != null
                           ? ((WorkType.GetCustomAttribute(typeof(SuitInfoAttribute)) as SuitInfoAttribute)?.Text ??
                              (WorkInstance as IInfoProvider)?.Text ??
                              (new SuitInfoAttribute(WorkInstance.GetType().Name)).Text)
                           : prompt;
            }

            if (!ShowReference || InstanceNameString.Count <= 0) return prompt;
            var sb = new StringBuilder();
            sb.Append(prompt);
            sb.Append('[');
            sb.Append(InstanceNameString[0]);
            if (InstanceNameString.Count > 1)
                for (var i = 1; i < InstanceNameString.Count; i++)
                    sb.Append($".{InstanceNameString[i]}");
            sb.Append(']');
            return sb.ToString();

        }


        private TraceBack RunBuildInCommand(string[] cmdList)
        {
            if (cmdList is null) return TraceBack.InvalidCommand;
            return BicServer.Execute(cmdList, out _);
        }
        /// <summary>
        /// whether mobile Suit shows command return value or not.
        /// </summary>
        public bool ShowReturnValue { get; set; }

        private TraceBack RunObject(string[] args)
        {
            var r = Current.Execute(args, out var result);
            if (!(result is null) && r == TraceBack.AllOk)
            {
                var retVal = result.ToString() ?? "";
                if (!string.IsNullOrEmpty(retVal)) _returnValue = retVal;
                if (ShowReturnValue) IO.WriteLine(IIOServer.CreateContentArray
                 (
                 (Lang.ReturnValue + ' ' + '>' + ' ', IO.ColorSetting.PromptColor),
                     (retVal, null)
                     )
                 );
            }

            return r;

        }
        /// <summary>
        /// Run a Mobile Suit with Prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns>0, is All ok.</returns>
        public int Run(string prompt)
        {
            Prompt.Update("", UpdatePrompt(prompt), TraceBack.AllOk);
            for (; ; )
            {
                if (!IO.IsInputRedirected) Prompt.Print();
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
                    case TraceBack.InvalidCommand:
                        NotifyError(Lang.InvalidCommand);
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
        /// <param name="withPrompt">if this run contains prompt, or silent</param>
        /// <param name="scriptName">name of these scripts</param>
        /// <returns>The TraceBack of the last executed command.</returns>
        public TraceBack RunScripts(IEnumerable<string> scripts, bool withPrompt = false, string? scriptName = null)
        {
            var i = 1;
            if (scripts == null) return TraceBack.AllOk;
            foreach (var script in scripts)
            {
                if (script is null) break;
                if (withPrompt)
                {
                    IO.WriteLine(IIOServer.CreateContentArray(
                        ("<Script:", IO.ColorSetting.PromptColor),
                        (scriptName ?? "(UNKNOWN)", IO.ColorSetting.InformationColor),
                        (">", IO.ColorSetting.PromptColor),
                        (script, null)));
                }
                var traceBack = RunCommand("", script);
                if (traceBack != TraceBack.AllOk)
                {
                    if (withPrompt)
                    {
                        IO.WriteLine(IIOServer.CreateContentArray(
                                ("TraceBack:", null),
                                (traceBack.ToString(), IO.ColorSetting.InformationColor),
                                (" at line ", null),
                                (i.ToString(CultureInfo.CurrentCulture), IO.ColorSetting.InformationColor)
                            ),
                            OutputType.Error);
                    }
                    return traceBack;
                }
                i++;
            }

            return TraceBack.AllOk;
        }
        /// <summary>
        /// Asynchronously run some SuitCommands in current environment, until one of them returns a non-AllOK TraceBack.
        /// </summary>
        /// <param name="scripts">SuitCommands</param>
        /// <param name="withPrompt">if this run contains prompt, or silent</param>
        /// <param name="scriptName">name of these scripts</param>
        /// <returns>The TraceBack of the last executed command.</returns>
        public async Task<TraceBack> RunScriptsAsync(IAsyncEnumerable<string?> scripts, bool withPrompt = false, string? scriptName = null)
        {
            var i = 1;
            if (scripts == null) return TraceBack.AllOk;
            await foreach (var script in scripts)
            {
                if (script is null) break;
                if (withPrompt)
                {
                    IO.WriteLine(IIOServer.CreateContentArray(
                        ("<Script:", IO.ColorSetting.PromptColor),
                        (scriptName ?? "(UNKNOWN)", IO.ColorSetting.InformationColor),
                        (">", IO.ColorSetting.PromptColor),
                        (script, null)));
                }

                var traceBack = RunCommand("", script);
                if (traceBack != TraceBack.AllOk)
                {
                    if (withPrompt)
                    {
                        IO.WriteLine(IIOServer.CreateContentArray(
                                ("TraceBack:", null),
                                (traceBack.ToString(), IO.ColorSetting.InformationColor),
                                (" at line ", null),
                                (i.ToString(CultureInfo.CurrentCulture), IO.ColorSetting.InformationColor)
                            ),
                            OutputType.Error);
                    }

                    return traceBack;
                }

                i++;
            }

            return TraceBack.AllOk;
        }

        private string _returnValue;

        private TraceBack RunCommand(string prompt, string? cmd)
        {
            if (IsNullOrEmpty(cmd) && IO.IsInputRedirected && ShellMode)
            {
                IO.ResetInput();
                return TraceBack.AllOk;
            }

            if (IsNullOrEmpty(cmd)) return TraceBack.AllOk;
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
                else
                {
                    traceBack = RunObject(args);
                    if (traceBack == TraceBack.ObjectNotFound) traceBack = RunBuildInCommand(args);
                }


            }
            catch (Exception e)
            {
                if (!UseTraceBack) throw;
                IO.ErrorStream.WriteLine(e.ToString());
                traceBack = TraceBack.InvalidCommand;
            }

            Prompt.Update(_returnValue, UpdatePrompt(prompt), traceBack);
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