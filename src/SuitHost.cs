#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Logging;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.UI;
using static System.String;
using Microsoft.Extensions.Logging;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     A entity, which serves the shell functions of a mobile-suit program.
    /// </summary>
    internal class SuitHost : IMobileSuitHost
    {
        private readonly ISuitAppBuilder _suitApp;
        private readonly IServiceScope _rootScope;
        private readonly ISuitExceptionHandler _exceptionHandler;
        private readonly IHost _delegateHost;
        private IIOHub IO { get; }
        private CancellationTokenSource systemInterruption;
        public SuitHost(IHost delegateHost)
        {
            _delegateHost = delegateHost;
            Services = delegateHost.Services;
            _suitApp = Services.GetRequiredService<ISuitAppBuilder>();
            _exceptionHandler = Services.GetService<ISuitExceptionHandler>() ?? ISuitExceptionHandler.Default();
            _rootScope = Services.CreateScope();
            IO = _rootScope.ServiceProvider.GetRequiredService<IIOHub>();
            Logger = Services.GetRequiredService<ILogger<SuitHost>>();
            systemInterruption = new();
        }

        private class SuitHostStatus : IHostStatus
        {
            /// <inheritdoc></inheritdoc>
            public RequestStatus TraceBack { get; set; } = RequestStatus.AllOk;
            /// <inheritdoc></inheritdoc>
            public object? ReturnValue { get; set; }
        }
        private object? _returnValue;

        /// <summary>
        ///     Initialize a SuitHost with instance, logger, io, bic-server and prompt-server
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="logger"></param>
        /// <param name="io"></param>
        /// <param name="buildInCommandServer"></param>
        public SuitHost(object? instance, ISuitLogger logger, IIOHub io, Type buildInCommandServer)
        //: this(configuration ?? ISuitConfiguration.GetDefaultConfiguration())
        {
            Current = new SuitShell(instance);
            Assembly = WorkType?.Assembly;
            Logger = logger;
            IO = io;
            Prompt = new();
            BicServer = new SuitShell(buildInCommandServer?.GetConstructor(new[] { typeof(SuitHost) })
                ?.Invoke(new object?[] { this }));
            WorkInstanceInit();
        }


        /// <summary>
        ///     Stack of Instance, created in this Mobile Suit.
        /// </summary>
        public Stack<SuitShell> InstanceStack { get; } = new Stack<SuitShell>();

        /// <summary>
        ///     String of Current Instance's Name.
        /// </summary>
        public List<string> InstanceNameString { get; } = new List<string>();

        /// <summary>
        ///     Stack of Instance's Name Strings.
        /// </summary>
        public Stack<List<string>> InstanceNameStringStack { get; } = new Stack<List<string>>();

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
        public AssignOncePromptGenerator Prompt { get; }

        /// <summary>
        ///     Current Instance's SuitObject Container.
        /// </summary>
        public SuitShell Current { get; set; }

        /// <summary>
        ///     Current BicServer's SuitObject Container.
        /// </summary>
        public SuitShell BicServer { get; set; }

        /// <summary>
        ///     Current Instance
        /// </summary>
        public object? WorkInstance => Current.Instance;

        /// <summary>
        ///     Current Instance's type.
        /// </summary>
        public Type? WorkType => Current.Instance?.GetType();




        /// <inheritdoc />
        public int Run()
        {
            _hostStatus.TraceBack = RequestStatus.AllOk;
            _hostStatus.ReturnValue = null;
            for (; ; )
            {
                var p = Prompt.GeneratePrompt();
                if (!IO.IsInputRedirected)
                    IO.Write(p, OutputType.Prompt);
                var traceBack = RunCommand(IO.ReadLine());
                switch (traceBack)
                {
                    case RequestStatus.OnExit:
                        (WorkInstance as IExitInteractive)?.OnExit();
                        return 0;
                    case RequestStatus.AllOk:
                        NotifyAllOk();
                        break;
                    case RequestStatus.ObjectNotFound:
                        NotifyError(Lang.ObjectNotFound);
                        break;
                    case RequestStatus.MemberNotFound:
                        NotifyError(Lang.MemberNotFound);
                        break;
                    case RequestStatus.InvalidCommand:
                        NotifyError(Lang.InvalidCommand);
                        break;
                    case RequestStatus.ApplicationError:
                        NotifyError(Lang.ApplicationError);
                        break;
                    default:
                        NotifyError(Lang.InvalidCommand);
                        break;
                }
            }
        }

        /// <inheritdoc />
        public RequestStatus RunScripts(IEnumerable<string> scripts, bool withPrompt = false, string? scriptName = null)
        {
            var i = 1;
            if (scripts == null) return RequestStatus.AllOk;
            foreach (var script in scripts)
            {
                if (script is null) break;
                if (withPrompt)
                    IO.WriteLine(IIOHub.CreateContentArray(
                        ("<Script:", IO.ColorSetting.PromptColor),
                        (scriptName ?? "(UNKNOWN)", IO.ColorSetting.InformationColor),
                        (">", IO.ColorSetting.PromptColor),
                        (script, null)));
                var traceBack = RunCommand(script);
                if (traceBack != RequestStatus.AllOk)
                {
                    if (withPrompt)
                        IO.WriteLine(IIOHub.CreateContentArray(
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

            return RequestStatus.AllOk;
        }


        /// <inheritdoc />
        public async Task<RequestStatus> RunScriptsAsync(IAsyncEnumerable<string?> scripts, bool withPrompt = false,
            string? scriptName = null)
        {
            var i = 1;
            if (scripts == null) return RequestStatus.AllOk;
            await foreach (var script in scripts)
            {
                if (script is null) break;
                if (withPrompt)
                    await IO.WriteLineAsync(IIOHub.CreateContentArray(
                        ("<Script:", IO.ColorSetting.PromptColor),
                        (scriptName ?? "(UNKNOWN)", IO.ColorSetting.InformationColor),
                        (">", IO.ColorSetting.PromptColor),
                        (script, null))).ConfigureAwait(false);

                var traceBack = RunCommand(script);
                if (traceBack != RequestStatus.AllOk)
                {
                    if (withPrompt)
                        await IO.WriteLineAsync(IIOHub.CreateContentArray(
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

            return RequestStatus.AllOk;
        }

        /// <inheritdoc />
        public RequestStatus RunCommand(string? cmd)
        {
            if (IsNullOrEmpty(cmd) && IO.IsInputRedirected && Settings.NoExit)
            {
                IO.ResetInput();
                return RequestStatus.AllOk;
            }

            if (IsNullOrEmpty(cmd) ||
                new Regex(@"^\s*#").IsMatch(cmd))
                return RequestStatus.AllOk;
            Logger.LogCommand(cmd);
            RequestStatus traceBack;
            var args = IMobileSuitHost.SplitCommandLine(cmd);
            if (args is null) return RequestStatus.InvalidCommand;
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
                    if (traceBack == RequestStatus.ObjectNotFound) traceBack = RunBuildInCommand(args);
                }
            }
            catch (Exception e)
            {
                if (Settings.EnableThrows) throw;
                IO.ErrorStream.WriteLine(e.ToString());
                traceBack = RequestStatus.InvalidCommand;
            }

            _hostStatus.ReturnValue = _returnValue;
            _hostStatus.TraceBack = traceBack;

            return traceBack;
        }

        /// <inheritdoc />
        public ILogger Logger { get; }



        /// <inheritdoc />
        public int Run(string[]? args)
        {
            if (args?.Length == 0) return Run();

            if (WorkInstance is ICommandLineApplication)
            {
                if (args == null || RunObject(args) != RequestStatus.AllOk)
                    return (WorkInstance as ICommandLineApplication)?.SuitStartUp(args) ?? 0;
                return 0;
            }

            if (args == null || RunObject(args) != RequestStatus.AllOk)
                return -1;
            return 0;
        }
        private readonly SuitHostStatus _hostStatus = new();
        /// <inheritdoc/>
        public IHostStatus HostStatus => _hostStatus;

        /// <inheritdoc />
        public HostSettings Settings { get; set; } = new();


        /// <summary>
        ///     Initialize the current instance, if it is a SuitClient, or implements IIOInteractive or ICommandInteractive.
        /// </summary>
        public void WorkInstanceInit()
        {
            (WorkInstance as IIOInteractive)?.IO.Assign(IO);
            (WorkInstance as IHostInteractive)?.Host.Assign(this);
            (WorkInstance as ILogInteractive)?.Log.Assign(Logger);
            (WorkInstance as IStartingInteractive)?.OnInitialized();
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


        private RequestStatus RunBuildInCommand(string[] cmdList)
        {
            var t = BicServer.Execute(cmdList, out var r);
            /*if (t == TraceBack.AllOk && r != null) _returnValue = r;*/

            if (r is Exception e) Logger.LogException(e);
            Logger.LogTraceBack(t, r as Exception);
            return t;
        }

        private RequestStatus RunObject(string[] args)
        {
            var t = Current.Execute(args, out var result);
            if (t == RequestStatus.AllOk && result != null)
            {
                _returnValue = result;
            }

            if (result is Exception e) Logger.LogException(e);
            Logger.LogTraceBack(t, result);

            return t;
        }

        public void Dispose()
        {
            _rootScope.Dispose();
            _delegateHost.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken = new())
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            cancellationToken.Register(() =>
            {
                systemInterruption.Cancel();
            });
            for (; ; )
            {
                systemInterruption = new();
                var requestScope = Services.CreateScope();
                var context = new SuitContext(requestScope, systemInterruption.Token);
                foreach (var middleware in _suitApp.Middlewares)
                {
                    if (systemInterruption.IsCancellationRequested) break;
                    try
                    {
                        await middleware.InvokeAsync(context);
                    }
                    catch (Exception ex)
                    {
                        context.Exception = ex;
                        context.Status = RequestStatus.ApplicationError;
                        await _exceptionHandler.InvokeAsync(context);
                        break;
                    }
                }
                if (context.Status == RequestStatus.NoRequest && systemInterruption.IsCancellationRequested)
                {

                }
            }

        }

        private void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            systemInterruption.Cancel();
        }

        public Task StopAsync(CancellationToken cancellationToken = new())
        {
            systemInterruption.Cancel();
            return Task.CompletedTask;
        }

        public IServiceProvider Services { get; }
    }
}