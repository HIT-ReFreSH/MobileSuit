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
using static PlasticMetal.MobileSuit.IO.IoInterface;
using PlasticMetal.MobileSuit.ObjectModel;

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
    
    public partial class MobileSuitHost
    {
        public static readonly Type ArgumentConverter = typeof(ArgumentConverterAttribute);

        private static string[]? ArgSplit(string args)
        {
            if (string.IsNullOrEmpty(args)) return null;
            string submit;
            var l = new List<string>();
            var separating = false;
            var separationPrefix = false;
            var separationCharacter = '"';
            var left = 0;
            var right = 0;
            for (; right < args.Length; right++)
            {
                switch (args[right])
                {
                    case '"':
                        if (separationPrefix) continue;
                        if (separating && separationCharacter == '"')
                        {
                            l.Add(args[left..right]);
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
                            l.Add(args[left..right]);
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
                        submit = args[left..right];
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

            submit = args[left..right];
            if (!string.IsNullOrEmpty(submit))
                l.Add(submit);
            return l.ToArray();
        }
        public Stack<MobileSuitObject> InstanceRef { get; set; } = new Stack<MobileSuitObject>();
        public List<string> InstanceNameStk { get; set; } = new List<string>();
        public bool ShowRef { get; set; } = true;
        public IoInterface Io { get; set; }
        public static IoInterface GeneralIo { get; set; } = new IoInterface();

        public Assembly? Assembly { get; set; }
        public string? Prompt { get; set; }
        public MobileSuitObject Current { get; set; }
        public object? WorkInstance => Current.Instance;
        public Type? WorkType => Current.Instance?.GetType();
        public MobileSuitHost(IoInterface? io = null)
        {
            Assembly = Assembly.GetCallingAssembly();
            Io = io ?? GeneralIo;
            Current = new MobileSuitObject(null);
        }
        public MobileSuitHost(object? instance, IoInterface? io = null)
        {

            Io = io ?? GeneralIo;
            Current = new MobileSuitObject(instance);
            Assembly = WorkType?.Assembly;

            WorkInstanceInit();
        }

        public MobileSuitHost(Assembly assembly, IoInterface? io = null)
        {
            Assembly = assembly;
            Io = io ?? GeneralIo;
            Current = new MobileSuitObject(null);
        }
        public MobileSuitHost(Type type, IoInterface? io = null)
        {

            Io = io ?? GeneralIo;
            if (type?.FullName == null)
            {
                Current = new MobileSuitObject(null);
                return;
            }
            Assembly = type.Assembly;
            Current = new MobileSuitObject(Assembly.CreateInstance(type.FullName));
            WorkInstanceInit();
        }

        private void WorkInstanceInit()
        {
            (WorkInstance as IIoInteractive)?.SetIo(Io);
            (WorkInstance as ICommandInteractive)?.SetCommandHandler(RunCommand);
        }
        public bool UseTraceBack { get; set; } = true;
        public bool ShowDone { get; set; }
        private void TbAllOk()
        {
            if (UseTraceBack && ShowDone) Io.WriteLine("Done.", IoInterface.OutputType.AllOk);
        }
        private void ErrInvalidCommand()
        {
            if (UseTraceBack)
            {
                Io.WriteLine("Invalid Command!", IoInterface.OutputType.Error);
            }
            else
            {
                throw new Exception("Invalid Command!");
            }
        }

        public bool ShellMode { get; set; } = false;

        private void ErrObjectNotFound()
        {
            if (UseTraceBack)
            {
                Io.WriteLine("Object Not Found!", IoInterface.OutputType.Error);
            }
            else
            {
                throw new Exception("Object Not Found!");
            }
        }
        private void ErrMemberNotFound()
        {
            if (UseTraceBack)
            {
                Io.WriteLine("Member Not Found!", IoInterface.OutputType.Error);
            }
            else
            {
                throw new Exception("Member Not Found!");
            }
        }
        private void UpdatePrompt(string prompt)
        {
            if (prompt == "" && WorkInstance != null)
            {
                if (WorkInstance is IInfoProvider)
                {
                    Prompt = ((IInfoProvider)WorkInstance).Prompt;
                }
                else
                {
                    if (WorkType != null)
                        Prompt =
                            (WorkType.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute
                             ?? new MobileSuitInfoAttribute(WorkInstance.GetType().Name)).Prompt;
                }
            }
            else
            {
                Prompt = prompt;
            }

            if (!ShowRef || InstanceNameStk.Count <= 0) return;
            var sb = new StringBuilder();
            sb.Append(Prompt);
            sb.Append('[');
            sb.Append(InstanceNameStk[0]);
            if (InstanceNameStk.Count > 1)
            {
                for (var i = 1; i < InstanceNameStk.Count; i++)
                {
                    sb.Append($".{InstanceNameStk[i]}");
                }
            }
            sb.Append(']');
            Prompt = sb.ToString();
            if (Io is null) return;
            Io.ConsoleTitle = Prompt;
        }

        private delegate void SetProp(object? obj, object? arg);


        private TraceBack RunBuildInCommand(string cmd)
        {

            var cmdList = ArgSplit(cmd);
            if (cmdList is null) return TraceBack.InvalidCommand;
            switch (cmdList[0].ToLower())
            {
                case "vw":
                case "view":
                    return cmdList.Length > 0 ? ViewObject(cmdList[1..]) : TraceBack.InvalidCommand;
                case "nw":
                case "new":
                    return cmdList.Length > 0 ? CreateObject(cmdList[1..]) : TraceBack.InvalidCommand;
                case "md":
                case "modify":
                    return cmdList.Length > 1 ? ModifyMember(cmdList[1..]) : TraceBack.InvalidCommand;
                case "rs":
                case "runscript":
                    if (cmdList.Length <= 1 || !File.Exists(cmdList[1])) return TraceBack.InvalidCommand;
                    RunScript(cmdList[1]);
                    return TraceBack.AllOk;
                case "lv":
                case "leave":
                    return LeaveObject(new[] { "" });
                case "et":
                case "enter":
                    return cmdList.Length > 0 ? EnterObjectMember(cmdList[1..]) : TraceBack.InvalidCommand;
                case "echo":
                    if (cmdList.Length == 1)
                    {
                        Io.WriteLine("");
                        return TraceBack.AllOk;
                    }
                    Io.WriteLine(cmd[(cmdList[0].Length + 1)..]);
                    return TraceBack.AllOk;
                case "echox":
                    if (cmdList.Length <= 2)
                    {
                        Io.WriteLine("");
                        return TraceBack.AllOk;
                    }
                    Io.WriteLine(cmd[(cmdList[0].Length + cmdList[1].Length + 2)..],
                        cmdList[1].ToLower() switch
                        {
                            "p" => OutputType.Prompt,
                            "prompt" => OutputType.Prompt,
                            "error" => OutputType.Error,
                            "err" => OutputType.Error,
                            "allok" => OutputType.AllOk,
                            "ok" => OutputType.AllOk,
                            "title" => OutputType.ListTitle,
                            "lt" => OutputType.ListTitle,
                            "custominfo" => OutputType.CustomInfo,
                            "info" => OutputType.CustomInfo,
                            "mobilesuitinfo" => OutputType.MobileSuitInfo,
                            "msi" => OutputType.MobileSuitInfo,
                            _ => OutputType.Default

                        },
                        typeof(ConsoleColor).
                            GetFields().
                            FirstOrDefault(
                                c => string.Equals(c.Name, cmdList[1],
                                    StringComparison.CurrentCultureIgnoreCase))
                            ?.GetValue(null) as ConsoleColor?
                    );
                    return TraceBack.AllOk;
                case "shell":
                    if (cmdList.Length < 2) return TraceBack.InvalidCommand;
                    var proc = new Process();
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.FileName = cmdList[1];
                    if (cmdList.Length > 2)
                    {

                        proc.StartInfo.Arguments = cmd[(cmdList[1].Length + cmdList[0].Length + 3)..];

                    }
                    try
                    {
                        proc.Start();
                    }
                    catch (Exception e)
                    {
                        Io.WriteLine($"Error:{e}", OutputType.Error);
                        return TraceBack.ObjectNotFound;
                    }

                    return TraceBack.AllOk;
                case "exit":
                    return TraceBack.OnExit;
                case "fr":
                case "free":
                    if (WorkType == null) return TraceBack.InvalidCommand;
                    Current = new MobileSuitObject(null);
                    Prompt = "";
                    InstanceRef.Clear();
                    InstanceNameStk.Clear();
                    return TraceBack.AllOk;
                case "ls":
                case "list":
                    return ListMembers();
                case "me":
                case "this":
                    if (WorkType == null) return TraceBack.InvalidCommand;
                    Io.WriteLine($"Work Instance:{WorkType.FullName}");
                    return TraceBack.AllOk;
                //case "modify":
                case "sw":
                case "switch":
                    return SwitchOption(cmdList[1]);
                default:
                    return TraceBack.InvalidCommand;
            }

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
                if (!Io.IsInputRedirected) Io.Write(Prompt + '>', IoInterface.OutputType.Prompt);

                if (RunCommand(prompt, Io.ReadLine()) == 0) return 0;

            }
        }

        public int RunScript(string path)
        {
            var t = RunScriptAsync(ReadTextFileAsync(path));
            t.Wait();
            return t.Result;
        }
        public int RunScript(IEnumerable<string> scripts)
        {
            foreach (var script in scripts)
            {
                RunCommand("", script);
            }
            return 0;
        }
        public async Task<int> RunScriptAsync(IAsyncEnumerable<string?> scripts)
        {
            await foreach (var script in scripts)
            {
                if (script is null) break;
                RunCommand("", script);
            }
            return 0;
        }
        public async IAsyncEnumerable<string?> ReadTextFileAsync(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists) throw new FileNotFoundException(fileName);
            var reader = fileInfo.OpenText();
            for (; ; )
                yield return await reader.ReadLineAsync();
        }


        public int RunCommand(string prompt, string? cmd)
        {
            if (string.IsNullOrEmpty(cmd) && (Io.IsInputRedirected && ShellMode))
            {
                Io.ResetInput();
                return 1;
            }

            if (string.IsNullOrEmpty(cmd)) return 1;

            try
            {

                TraceBack traceBack;
            if (cmd[0] == '@')
            {
                traceBack = RunBuildInCommand(cmd.Remove(0, 1));
            }
            else
            {
                var commandlineList = ArgSplit(cmd);
                traceBack = commandlineList == null
                    ? TraceBack.InvalidCommand
                    : RunObject(commandlineList);
                if (traceBack == TraceBack.ObjectNotFound)
                {
                    traceBack = RunBuildInCommand(cmd);
                }
            }
            switch (traceBack)
            {
                case TraceBack.OnExit:
                    return 0;
                case TraceBack.AllOk:
                    TbAllOk();
                    break;
                case TraceBack.InvalidCommand:
                    ErrInvalidCommand();
                    break;
                case TraceBack.ObjectNotFound:
                    ErrObjectNotFound();
                    break;
                case TraceBack.MemberNotFound:
                    ErrMemberNotFound();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            }
            catch (Exception e)
            {
                Io.Error.WriteLine(e.ToString());
            }
            UpdatePrompt(prompt);
            return 1;
        }
        public int Run()
            => Run("");
    }
}
