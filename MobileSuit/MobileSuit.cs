#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using static MobileSuit.MobileSuitIoInterface;

namespace MobileSuit
{

    public class MobileSuitHost
    {
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
        public Stack<object> InstanceRef { get; set; } = new Stack<object>();
        public List<string> InstanceNameStk { get; set; } = new List<string>();
        public bool ShowRef { get; set; } = true;
        public const BindingFlags Flags = BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
        public MobileSuitIoInterface Io { get; set; }
        public static MobileSuitIoInterface GeneralIo { get; set; } = new MobileSuitIoInterface();
        public enum TraceBack
        {

            OnExit = 1,
            AllOk = 0,
            InvalidCommand = -1,
            ObjectNotFound = -2,
            MemberNotFound = -3

        }
        public Assembly? Assembly { get; set; }
        public string? Prompt { get; set; }
        public object? WorkInstance { get; set; }
        public Type? WorkType { get; set; }
        public MobileSuitHost(MobileSuitIoInterface? io = null)
        {
            Assembly = Assembly.GetCallingAssembly();
            Io = io ?? GeneralIo;
        }


        public MobileSuitHost(Assembly assembly, MobileSuitIoInterface? io = null)
        {
            Assembly = assembly;
            Io = io ?? GeneralIo;
        }
        public MobileSuitHost(Type? type, MobileSuitIoInterface? io = null)
        {

            WorkType = type;
            Io = io ?? GeneralIo;
            if (type?.FullName == null) return;
            Assembly = type.Assembly;
            WorkInstance = Assembly.CreateInstance(type.FullName);
            WorkInstanceInit();
        }

        private void WorkInstanceInit()
        {
            (WorkInstance as IMobileSuitIoInteractive)?.SetIo(Io);
            (WorkInstance as IMobileSuitCommandInteractive)?.SetCommandHandler(RunCommand);
        }
        public bool UseTraceBack { get; set; } = true;
        public bool ShowDone { get; set; }
        private void TbAllOk()
        {
            if (UseTraceBack && ShowDone) Io.WriteLine("Done.", MobileSuitIoInterface.OutputType.AllOk);
        }
        private void ErrInvalidCommand()
        {
            if (UseTraceBack)
            {
                Io.WriteLine("Invalid Command!", MobileSuitIoInterface.OutputType.Error);
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
                Io.WriteLine("Object Not Found!", MobileSuitIoInterface.OutputType.Error);
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
                Io.WriteLine("Member Not Found!", MobileSuitIoInterface.OutputType.Error);
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
                if (WorkInstance is IMobileSuitInfoProvider)
                {
                    Prompt = ((IMobileSuitInfoProvider)WorkInstance).Prompt;
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
        }
        private TraceBack ListMembers()
        {
            if (WorkType == null) return TraceBack.InvalidCommand;
            var fi = (from i in (from f in WorkType.GetFields(Flags)
                                 select (MemberInfo)f).Union
                               (from p in WorkType.GetProperties(Flags)
                                select (MemberInfo)p)
                      orderby i.Name
                      select i).ToList();


            if (fi.Any())
            {
                Io.WriteLine("Members:", MobileSuitIoInterface.OutputType.ListTitle);
                foreach (var item in fi)
                {
                    var info = item.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute;
                    var exInfo = info == null
                                ? ""
                                : $"[{info.Prompt}]";
                    Io.Write($"\t{item.Name}");
                    var otType = info == null
                        ? MobileSuitIoInterface.OutputType.MobileSuitInfo
                        : MobileSuitIoInterface.OutputType.CustomInfo;
                    Io.Write(exInfo, otType);
                    Io.Write("\n");
                }
            }
            var mi = (from m in WorkType.GetMethods(Flags)
                      where
                            !(from p in WorkType.GetProperties(Flags)
                              select $"get_{p.Name}").Contains(m.Name)
                         && !(from p in WorkType.GetProperties(Flags)
                              select $"set_{p.Name}").Contains(m.Name)
                      select m).ToList();


            if (!mi.Any()) return TraceBack.AllOk;
            {
                Io.WriteLine("Methods:", MobileSuitIoInterface.OutputType.ListTitle);
                foreach (var item in mi)
                {
                    var info = item.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute;
                    var exInfo = info == null
                        ? $"({ item.GetParameters().Length} Parameters)"
                        : $"[{info.Prompt}]";
                    Io.Write($"\t{item.Name}");
                    var otType = info == null
                        ? MobileSuitIoInterface.OutputType.MobileSuitInfo
                        : MobileSuitIoInterface.OutputType.CustomInfo;
                    Io.Write(exInfo, otType);
                    Io.Write("\n");
                }
            }
            return TraceBack.AllOk;
        }
        private delegate void SetProp(object? obj, object? arg);
        private TraceBack SwitchOption(string optionName)
        {
            switch (optionName)
            {
                case "sr":
                case "ShowRef":
                    ShowRef = !ShowRef;
                    return TraceBack.AllOk;
                case "sd":
                case "ShowDone":
                    ShowDone = !ShowDone;
                    return TraceBack.AllOk;

                case "tb":
                case "TraceBack":
                    UseTraceBack = !UseTraceBack;
                    return TraceBack.AllOk;
                default:
                    return TraceBack.InvalidCommand;
            }
        }
        private TraceBack ModifyMember(string[] args)
        {
            if (WorkType == null) return TraceBack.ObjectNotFound;
            var obj = WorkType?.GetProperty(args[0], Flags) as MemberInfo ?? WorkType?.GetField(args[0], Flags);
            var objProp = WorkType?.GetProperty(args[0], Flags);
            if (obj == null || objProp == null) return TraceBack.MemberNotFound;
            var objSet = (SetProp)objProp.SetValue;

            var cvt = (obj.GetCustomAttribute(typeof(MobileSuitDataConverterAttribute)) as MobileSuitDataConverterAttribute)?.Converter;
            try
            {
                objSet(WorkInstance, cvt != null ? cvt(args[1]) : args[1]);
                return TraceBack.AllOk;
            }
            catch
            {
                return TraceBack.InvalidCommand;
            }
        }
        private TraceBack RunLocal(string cmd)
        {

            var cmdList = ArgSplit(cmd);
            if (cmdList is null) return TraceBack.InvalidCommand;
            switch (cmdList[0].ToLower())
            {
                case "vw":
                case "view":
                    if (cmdList.Length == 1) return TraceBack.InvalidCommand;
                    if (WorkType == null || Assembly == null) return TraceBack.InvalidCommand;

                    var obj = WorkType.GetProperty(cmdList[1], Flags)?.GetValue(WorkInstance) ??
                                WorkType.GetField(cmdList[1], Flags)?.GetValue(WorkInstance);
                    if (obj == null)
                    {
                        return TraceBack.ObjectNotFound;
                    }
                    Io.WriteLine(obj.ToString() ?? "<Unknown>");
                    return TraceBack.AllOk;
                case "nw":
                case "new":
                    if (cmdList.Length == 1 || Assembly == null) return TraceBack.InvalidCommand;
                    WorkType =
                        Assembly.GetType(cmdList[1], false, true) ??
                        Assembly.GetType(WorkType?.FullName + '.' + cmdList[1], false, true) ??
                        Assembly.GetType(Assembly.GetName().Name + '.' + cmdList[1], false, true);
                    if (WorkType == null)
                    {
                        return TraceBack.ObjectNotFound;
                    }

                    if (WorkType?.FullName == null) return TraceBack.InvalidCommand;
                    WorkInstance = Assembly.CreateInstance(WorkType?.FullName ?? throw new NullReferenceException());
                    Prompt = (WorkType?.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute
                        ?? new MobileSuitInfoAttribute(cmdList[1])).Prompt;
                    InstanceRef.Clear();
                    InstanceNameStk.Clear();
                    InstanceNameStk.Add($"(new {WorkType?.Name})");
                    WorkInstanceInit();
                    return TraceBack.AllOk;
                case "md":
                case "modify":
                    if (WorkType == null || Assembly == null) return TraceBack.InvalidCommand;
                    return cmdList.Length == 1 ? TraceBack.InvalidCommand : ModifyMember(cmdList[1..]);
                case "lv":
                case "leave":
                    if (InstanceRef.Count == 0)
                        return TraceBack.InvalidCommand;
                    if (WorkType == null) return TraceBack.InvalidCommand;
                    WorkInstance = InstanceRef.Pop();
                    InstanceNameStk.RemoveAt(InstanceNameStk.Count - 1);//PopBack
                    WorkType = WorkInstance.GetType();
                    WorkInstanceInit();
                    return TraceBack.AllOk;
                case "et":
                case "enter":
                    if (cmdList.Length == 1) return TraceBack.InvalidCommand;
                    if (WorkType == null || WorkInstance == null || Assembly == null) return TraceBack.InvalidCommand;
                    var nextobj = WorkType.GetProperty(cmdList[1], Flags)?.GetValue(WorkInstance) ??
                        WorkType.GetField(cmdList[1], Flags)?.GetValue(WorkInstance);
                    InstanceRef.Push(WorkInstance);
                    var fName = WorkType?.GetProperty(cmdList[1], Flags)?.Name ??
                                WorkType?.GetField(cmdList[1], Flags)?.Name;
                    if (fName == null || nextobj == null) return TraceBack.ObjectNotFound;
                    InstanceNameStk.Add(fName);
                    WorkInstance = nextobj;
                    WorkType = nextobj.GetType();
                    WorkInstanceInit();
                    return TraceBack.AllOk;
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
                case "systemcall":
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
                    WorkType = null;
                    WorkInstance = null;
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
        private TraceBack RunObject(string[] cmdlist, object? instance)
        {
            if (Assembly == null) return TraceBack.InvalidCommand;
            if (0 == cmdlist.Length)
            {
                return TraceBack.ObjectNotFound;
            }
            if (instance != null)
            {
                var type = instance.GetType();
                MethodInfo mi;
                try
                {
                    mi = type
                        .GetMethods(Flags)
                        .Where(m => string.Equals(m.Name, cmdlist[0], StringComparison.CurrentCultureIgnoreCase))
                        .FirstOrDefault(m => m.GetParameters().Length == cmdlist.Length - 1);
                }
                catch (AmbiguousMatchException)
                {

                    return TraceBack.InvalidCommand;
                }

                //if there's no such method
                if (mi == null)
                {
                    var nextobj =
                        type.GetProperty(cmdlist[0], Flags)?.GetValue(instance) ??
                        type.GetField(cmdlist[0], Flags)?.GetValue(instance) ??
                        type.Assembly.CreateInstance(type.FullName + "." + cmdlist[0]);
                    return RunObject(cmdlist[1..], nextobj);
                }

                mi.Invoke(instance, cmdlist[1..]);
                return TraceBack.AllOk;
            }

            if (WorkInstance is null)
            {
                var nextObject = Assembly.GetType(cmdlist[0], false, true) ??
                              Assembly.GetType(Assembly.GetName().Name + '.' + cmdlist[0], false, true);
                if (nextObject == null)
                {
                    return TraceBack.ObjectNotFound;
                }

                return RunObject(cmdlist[1..],
                    Assembly.CreateInstance(nextObject.FullName ?? throw new InvalidOperationException()));
            }
            //If Null Instance

            return TraceBack.ObjectNotFound;

        }
        public int Run(string prompt)
        {
            UpdatePrompt(prompt);
            for (; ; )
            {
                if (!Io.IsInputRedirected) Io.Write(Prompt + '>', MobileSuitIoInterface.OutputType.Prompt);

                if (RunCommand(prompt, Io.ReadLine()) == 0) return 0;

            }
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
                    traceBack = RunLocal(cmd.Remove(0, 1));
                }
                else
                {
                    var commandlineList = ArgSplit(cmd);
                    traceBack = commandlineList == null
                        ? TraceBack.InvalidCommand
                        : RunObject(commandlineList, WorkInstance);

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
