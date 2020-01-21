#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit
{
    public enum TraceBack
    {

        OnExit = 1,
        AllOk = 0,
        InvalidCommand = -1,
        ObjectNotFound = -2,
        MemberNotFound = -3

    }

    public class MsHost
    {

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
            {
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
            }

            submit = commandLine[left..right];
            if (!string.IsNullOrEmpty(submit))
                l.Add(submit);
            return l.ToArray();
        }
        public Stack<MsObject> InstanceRef { get; set; } = new Stack<MsObject>();
        public List<string> InstanceNames { get; set; } = new List<string>();
        public Stack<List<string>> InstanceNamesRef { get; set; } = new Stack<List<string>>();
        public bool ShowRef { get; set; } = true;
        public IoServer Io { get; set; }
        public static IoServer GeneralIo { get; set; } = new IoServer();

        public Assembly? Assembly { get; set; }
        public string? Prompt { get; set; }
        public MsObject Current { get; set; }
        public MsObject BicServer { get; set; }
        public object? WorkInstance => Current.Instance;
        public Type? WorkType => Current.Instance?.GetType();
        public MsHost(IoServer? io = null)
        {
            Assembly = Assembly.GetCallingAssembly();
            Io = io ?? GeneralIo;
            Current = new MsObject(null);
            BicServer = new MsObject(new MsBicServer(this));
        }
        public MsHost(object? instance, IoServer? io = null) : this(io)
        {

            Current = new MsObject(instance);
            Assembly = WorkType?.Assembly;

            WorkInstanceInit();
        }

        public MsHost(Assembly assembly, IoServer? io = null) : this(io)
        {
            Assembly = assembly;
            Current = new MsObject(null);
        }
        public MsHost(Type type, IoServer? io = null) : this(io)
        {

            if (type?.FullName == null)
            {
                Current = new MsObject(null);
                return;
            }
            Assembly = type.Assembly;
            Current = new MsObject(Assembly.CreateInstance(type.FullName));
            WorkInstanceInit();
        }

        internal void WorkInstanceInit()
        {
            (WorkInstance as IIoInteractive)?.SetIo(Io);
            (WorkInstance as ICommandInteractive)?.SetCommandHandler(RunCommand);
        }
        public bool UseTraceBack { get; set; } = true;
        public bool ShowDone { get; set; }
        private void NotifyAllOk()
        {
            if (UseTraceBack && ShowDone) Io.WriteLine("Done.", OutputType.AllOk);
        }


        public bool ShellMode { get; set; } = false;



        private void NotifyError(string errorDescription)
        {
            if (UseTraceBack)
            {
                Io.WriteLine(errorDescription, OutputType.Error);
            }
            else
            {
                throw new Exception(errorDescription);
            }
        }
        private void UpdatePrompt(string prompt)
        {
            if (prompt == "" && WorkInstance != null)
            {
                if (WorkInstance is IInfoProvider)
                {
                    Prompt = ((IInfoProvider)WorkInstance).Text;
                }
                else
                {
                    if (WorkType != null)
                        Prompt =
                            (WorkType.GetCustomAttribute(typeof(MsInfoAttribute)) as MsInfoAttribute
                             ?? new MsInfoAttribute(WorkInstance.GetType().Name)).Text;
                }
            }
            else
            {
                Prompt = prompt;
            }

            if (!ShowRef || InstanceNames.Count <= 0) return;
            var sb = new StringBuilder();
            sb.Append(Prompt);
            sb.Append('[');
            sb.Append(InstanceNames[0]);
            if (InstanceNames.Count > 1)
            {
                for (var i = 1; i < InstanceNames.Count; i++)
                {
                    sb.Append($".{InstanceNames[i]}");
                }
            }
            sb.Append(']');
            Prompt = sb.ToString();
            if (Io is null) return;
            Io.ConsoleTitle = Prompt;
        }




        private TraceBack RunBuildInCommand(string cmd)
        {

            var cmdList = SplitCommandLine(cmd);
            if (cmdList is null) return TraceBack.InvalidCommand;
            return BicServer.Execute(cmdList);


        }
        private TraceBack RunObject(string[] args)
        {
            return Current.Execute(args);
        }
        public int Run(string prompt)
        {
            UpdatePrompt(prompt);
            for (; ; )
            {
                if (!Io.IsInputRedirected) Io.Write(Prompt + '>', OutputType.Prompt);
                var traceBack = RunCommand(prompt, Io.ReadLine());
                switch (traceBack)
                {
                    case TraceBack.OnExit:
                        return 0;
                    case TraceBack.AllOk:
                        NotifyAllOk();
                        break;
                    case TraceBack.ObjectNotFound:
                        NotifyError("Object Not Found!");
                        break;
                    case TraceBack.MemberNotFound:
                        NotifyError("Member Not Found!");
                        break;
                    default:
                        NotifyError("Invalid Command!");
                        break;
                }


            }
        }
        public TraceBack RunScript(IEnumerable<string> scripts)
        {
            foreach (var script in scripts)
            {
                if (script is null) break;
                var traceBack = RunCommand("", script);
                if (traceBack != TraceBack.AllOk)
                {
                    return traceBack;
                }
            }
            return TraceBack.AllOk;
        }
        public async Task<TraceBack> RunScriptAsync(IAsyncEnumerable<string?> scripts)
        {
            await foreach (var script in scripts)
            {
                if (script is null) break;
                var traceBack = RunCommand("", script);
                if (traceBack!=TraceBack.AllOk)
                {
                    return traceBack;
                }
            }
            return TraceBack.AllOk;
        }
        public TraceBack RunCommand(string prompt, string? cmd)
        {
            if (string.IsNullOrEmpty(cmd) && (Io.IsInputRedirected && ShellMode))
            {
                Io.ResetInput();
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
                    traceBack = BicServer.Execute(args);
                }

                traceBack = RunObject(args);
                if (traceBack == TraceBack.ObjectNotFound)
                {
                    traceBack = RunBuildInCommand(cmd);
                }

            }
            catch (Exception e)
            {
                Io.Error.WriteLine(e.ToString());
                traceBack = TraceBack.InvalidCommand;
            }
            UpdatePrompt(prompt);
            return traceBack;
        }
        public int Run()
            => Run("");
    }
}
