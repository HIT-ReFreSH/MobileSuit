#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;
using static System.String;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     A entity, which serves the shell functions of a mobile-suit program.
    /// </summary>
    public class SuitHost : IMobileSuitHost
    {
        private object? _returnValue;

        /// <summary>
        /// Initialize a SuitHost with instance, logger, io, bic-server and prompt-server
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="logger"></param>
        /// <param name="io"></param>
        /// <param name="buildInCommandServer"></param>
        /// <param name="prompt"></param>
        public SuitHost(object? instance, Logger logger, IIOServer io, Type buildInCommandServer, IPromptServer prompt)
        //: this(configuration ?? ISuitConfiguration.GetDefaultConfiguration())
        {
            Current = new SuitObject(instance);
            Assembly = WorkType?.Assembly;
            Logger = logger;
            IO = io;
            Prompt = prompt;
            BicServer = new SuitObject(buildInCommandServer?.GetConstructor(new[] { typeof(SuitHost) })?.Invoke(new object?[] { this }));
            WorkInstanceInit();
        }

        ///// <summary>
        /////     Initialize a SuitHost with given configuration, Calling Assembly.
        ///// </summary>
        ///// <param name="configuration">given configuration</param>
        //public SuitHost(ISuitConfiguration configuration)
        //{
        //    _returnValue = "";
        //    Configuration = configuration ?? ISuitConfiguration.GetDefaultConfiguration();
        //    Assembly = Assembly.GetCallingAssembly();
        //    Current = new SuitObject(null);
        //    Configuration.InitializeBuildInCommandServer(this);
        //    BicServer = new SuitObject(Configuration.BuildInCommandServer);
        //    IO.ColorSetting = Configuration.ColorSetting;
        //    IO.Prompt = Prompt;
        //}

        ///// <summary>
        /////     Initialize a SuitHost with given/default configuration,  an instance, and its type's Assembly.
        ///// </summary>
        ///// <param name="instance">The instance for Mobile Suit to drive.</param>
        ///// <param name="configuration">given configuration</param>
        //public SuitHost(object? instance, ISuitConfiguration? configuration = null)
        //    //: this(configuration ?? ISuitConfiguration.GetDefaultConfiguration())
        //{
        //    Current = new SuitObject(instance);
        //    Assembly = WorkType?.Assembly;

        //    WorkInstanceInit();
        //}

        ///// <summary>
        /////     Initialize a SuitHost with given/default configuration, given Assembly.
        ///// </summary>
        ///// <param name="assembly">The given Assembly.</param>
        ///// <param name="configuration">given configuration, default if null</param>
        //public SuitHost(Assembly assembly, ISuitConfiguration? configuration) : this(
        //    configuration ?? ISuitConfiguration.GetDefaultConfiguration())
        //{
        //    Assembly = assembly;
        //    Current = new SuitObject(null);
        //}

        ///// <summary>
        /////     Initialize a SuitHost with given configuration,  a given type, and its  Assembly.
        ///// </summary>
        ///// <param name="type">The given Type</param>
        ///// <param name="configuration">given configuration, default if null</param>
        //public SuitHost(Type type, ISuitConfiguration? configuration) : this(
        //    configuration ?? ISuitConfiguration.GetDefaultConfiguration())
        //{
        //    if (type?.FullName == null)
        //    {
        //        Current = new SuitObject(null);
        //        return;
        //    }

        //    Assembly = type.Assembly;
        //    Current = new SuitObject(Assembly.CreateInstance(type.FullName));
        //    WorkInstanceInit();
        //}

        /// <summary>
        ///     Stack of Instance, created in this Mobile Suit.
        /// </summary>
        public Stack<SuitObject> InstanceStack { get; } = new Stack<SuitObject>();

        /// <summary>
        ///     String of Current Instance's Name.
        /// </summary>
        public List<string> InstanceNameString { get; } = new List<string>();

        /// <summary>
        ///     Stack of Instance's Name Strings.
        /// </summary>
        public Stack<List<string>> InstanceNameStringStack { get; } = new Stack<List<string>>();



        /// <summary>
        ///     The IOServer for this SuitHost
        /// </summary>
        public IIOServer IO { get; }

        ///// <summary>
        /////     The configuration used to initialize the mobile suit
        ///// </summary>
        //public ISuitConfiguration Configuration { get; }

        /// <summary>
        ///     The Assembly which instance are from.
        /// </summary>
        public Assembly? Assembly { get; set; }

        /// <summary>
        ///     The prompt in Console.
        /// </summary>
        public IPromptServer Prompt { get; }

        /// <summary>
        ///     Current Instance's SuitObject Container.
        /// </summary>
        public SuitObject Current { get; set; }

        /// <summary>
        ///     Current BicServer's SuitObject Container.
        /// </summary>
        public SuitObject BicServer { get; set; }

        /// <summary>
        ///     Current Instance
        /// </summary>
        public object? WorkInstance => Current.Instance;

        /// <summary>
        ///     Current Instance's type.
        /// </summary>
        public Type? WorkType => Current.Instance?.GetType();





        /// <summary>
        ///     Initialize the current instance, if it is a SuitClient, or implements IIOInteractive or ICommandInteractive.
        /// </summary>
        public void WorkInstanceInit()
        {
            (WorkInstance as IIOInteractive)?.IO.Assign(IO);
            (WorkInstance as IHostInteractive)?.Host.Assign(this);
            (WorkInstance as ILogInteractive)?.Log.Assign(Logger);
        }

        private void NotifyAllOk()
        {
            if (!Settings.EnableThrows && Settings.ShowDone) IO.WriteLine(Lang.Done, OutputType.AllOk);
        }


        private void NotifyError(string errorDescription)
        {
            if (!Settings.EnableThrows) IO.WriteLine(errorDescription + '!', OutputType.Error);
            else throw new Exception(errorDescription);
        }

        private string UpdatePrompt(string prompt)
        {
            if (IsNullOrEmpty(prompt) && WorkInstance != null)
                return WorkType != null
                    ? (WorkType.GetCustomAttribute(typeof(SuitInfoAttribute)) as SuitInfoAttribute)?.Text ??
                      (WorkInstance as IInfoProvider)?.Text ??
                      new SuitInfoAttribute(WorkInstance.GetType().Name).Text
                    : prompt;

            if (Settings.HideReference || InstanceNameString.Count <= 0) return prompt;
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
            object? r = null;
            var t = cmdList is null ? TraceBack.InvalidCommand : BicServer.Execute(cmdList, out r);
            if (t == TraceBack.AllOk && r != null)
            {
                _returnValue = r;
            }

            if (r is Exception e)
            {
                Logger.LogException(e);
            }
            Logger.LogTraceBack(t, r as Exception);
            return t;
        }

        private TraceBack RunObject(string[] args)
        {
            var t = Current.Execute(args, out var result);
            if (t == TraceBack.AllOk && result != null)
            {

                _returnValue = result;
                if (!Settings.HideReturnValue)
                    IO.WriteLine(IIOServer.CreateContentArray
                        (
                            (Lang.ReturnValue + ' ' + '>' + ' ', IO.ColorSetting.PromptColor),
                            (result.ToString()??"", null)
                        )
                    );
            }
            if (result is Exception e)
            {
                Logger.LogException(e);
            }
            Logger.LogTraceBack(t, result);

            return t;
        }


        /// <inheritdoc />
        public int Run(string prompt)
        {
            Prompt.Update("", UpdatePrompt(prompt), TraceBack.AllOk);
            for (; ; )
            {
                if (!IO.IsInputRedirected) Prompt.Print();
                var traceBack = RunCommand(IO.ReadLine(), prompt);
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
                    case TraceBack.ApplicationError:
                        NotifyError(Lang.ApplicationError);
                        break;
                    default:
                        NotifyError(Lang.InvalidCommand);
                        break;
                }
            }
        }

        /// <inheritdoc/>
        public TraceBack RunScripts(IEnumerable<string> scripts, bool withPrompt = false, string? scriptName = null)
        {
            var i = 1;
            if (scripts == null) return TraceBack.AllOk;
            foreach (var script in scripts)
            {
                if (script is null) break;
                if (withPrompt)
                    IO.WriteLine(IIOServer.CreateContentArray(
                        ("<Script:", IO.ColorSetting.PromptColor),
                        (scriptName ?? "(UNKNOWN)", IO.ColorSetting.InformationColor),
                        (">", IO.ColorSetting.PromptColor),
                        (script, null)));
                var traceBack = RunCommand(script);
                if (traceBack != TraceBack.AllOk)
                {
                    if (withPrompt)
                        IO.WriteLine(IIOServer.CreateContentArray(
                                ("TraceBack:", null),
                                (traceBack.ToString(), IO.ColorSetting.InformationColor),
                                (" at line ", null),
                                (i.ToString(CultureInfo.CurrentCulture), IO.ColorSetting.InformationColor)
                            ),
                            OutputType.Error);
                    return traceBack;
                }

                i++;
            }

            return TraceBack.AllOk;
        }


        /// <inheritdoc />
        public async Task<TraceBack> RunScriptsAsync(IAsyncEnumerable<string?> scripts, bool withPrompt = false,
            string? scriptName = null)
        {
            var i = 1;
            if (scripts == null) return TraceBack.AllOk;
            await foreach (var script in scripts)
            {
                if (script is null) break;
                if (withPrompt)
                    await IO.WriteLineAsync(IIOServer.CreateContentArray(
                        ("<Script:", IO.ColorSetting.PromptColor),
                        (scriptName ?? "(UNKNOWN)", IO.ColorSetting.InformationColor),
                        (">", IO.ColorSetting.PromptColor),
                        (script, null))).ConfigureAwait(false);

                var traceBack = RunCommand(script);
                if (traceBack != TraceBack.AllOk)
                {
                    if (withPrompt)
                        await IO.WriteLineAsync(IIOServer.CreateContentArray(
                                ("TraceBack:", null),
                                (traceBack.ToString(), IO.ColorSetting.InformationColor),
                                (" at line ", null),
                                (i.ToString(CultureInfo.CurrentCulture), IO.ColorSetting.InformationColor)
                            ),
                            OutputType.Error).ConfigureAwait(false);

                    return traceBack;
                }

                i++;
            }

            return TraceBack.AllOk;
        }
        /// <inheritdoc />
        public TraceBack RunCommand(string? cmd, string prompt = "")
        {
            if (IsNullOrEmpty(cmd) && IO.IsInputRedirected && Settings.NoExit)
            {
                IO.ResetInput();
                return TraceBack.AllOk;
            }

            if (IsNullOrEmpty(cmd)) return TraceBack.AllOk;
            Logger.LogCommand(cmd);
            TraceBack traceBack;
            var args = IMobileSuitHost.SplitCommandLine(cmd);
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
                if (Settings.EnableThrows) throw;
                IO.ErrorStream.WriteLine(e.ToString());
                traceBack = TraceBack.InvalidCommand;
            }

            Prompt.Update(_returnValue?.ToString() ?? "", UpdatePrompt(prompt), traceBack);
            return traceBack;
        }

        /// <inheritdoc />
        public Logger Logger { get; }


        /// <inheritdoc />
        public int Run()
        {
            return Run("");
        }

        /// <inheritdoc />
        public HostSettings Settings { get; set; } = new HostSettings();
    }
}