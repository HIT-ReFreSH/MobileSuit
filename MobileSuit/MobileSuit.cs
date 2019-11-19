#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.IO;

namespace MobileSuit
{
    public class MobileSuit
    {
        public Stack<object> InstanceRef { get; set; } = new Stack<object>();
        public List<string> InstanceNameStk { get; set; } = new List<string>();
        public bool ShowRef { get; set; } = true;
        public const BindingFlags Flags = BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
        public MobileSuitIoInterface IO { get; set; } = new MobileSuitIoInterface();
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
        public MobileSuit()
        {
            Assembly = Assembly.GetCallingAssembly();
            
        }



        public MobileSuit(Assembly assembly)
            => Assembly = assembly;
        public MobileSuit(Type? type)
        {

            WorkType = type;
            if (type?.FullName == null) return;
            Assembly = type.Assembly;

            WorkInstance = Assembly.CreateInstance(type.FullName);
        }

        public bool UseTraceBack { get; set; } = true;
        public bool ShowDone { get; set; }
        private void TbAllOk()
        {
            if (UseTraceBack && ShowDone) IO.WriteLine("Done.", MobileSuitIoInterface.OutputType.AllOk);
        }
        private void ErrInvalidCommand()
        {
            if (UseTraceBack)
            {
                IO.WriteLine("Invalid Command!", MobileSuitIoInterface.OutputType.Error);
            }
            else
            {
                throw new Exception("Invalid Command!");
            }
        }


        private void ErrObjectNotFound()
        {
            if (UseTraceBack)
            {
                IO.WriteLine("Object Not Found!", MobileSuitIoInterface.OutputType.Error);
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
                IO.WriteLine("Member Not Found!", MobileSuitIoInterface.OutputType.Error);
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
                IO.WriteLine("Members:",MobileSuitIoInterface.OutputType.ListTitle);
                foreach (var item in fi)
                {
                    var info = item.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute;
                    var exInfo = info == null
                                ? ""
                                : $"[{info.Prompt}]";
                    IO.Write($"\t{item.Name}");
                    var otType = info == null
                        ? MobileSuitIoInterface.OutputType.MobileSuitInfo
                        : MobileSuitIoInterface.OutputType.CustomInfo;
                    IO.Write(exInfo, otType);
                    IO.Write("\n");
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
                IO.WriteLine("Methods:",MobileSuitIoInterface.OutputType.ListTitle);
                foreach (var item in mi)
                {
                    var info = item.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute;
                    var exInfo = info == null
                        ? $"({ item.GetParameters().Length} Parameters)"
                        : $"[{info.Prompt}]";
                    IO.Write($"\t{item.Name}");
                    var otType = info == null
                        ? MobileSuitIoInterface.OutputType.MobileSuitInfo
                        : MobileSuitIoInterface.OutputType.CustomInfo;
                    IO.Write(exInfo, otType);
                    IO.Write("\n");
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

                case "utb":
                case "UseTraceBack":
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
            var objSet = (SetProp) objProp.SetValue;

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

            var cmdlist = cmd.ToLower().Split(' ');
            switch (cmdlist[0])
            {
                case "vw":
                case "view":
                    if (cmdlist.Length == 1) return TraceBack.InvalidCommand;
                    if (WorkType == null||Assembly==null) return TraceBack.InvalidCommand;

                    var obj = WorkType.GetProperty(cmdlist[1], Flags)?.GetValue(WorkInstance) ??
                                WorkType.GetField(cmdlist[1], Flags)?.GetValue(WorkInstance);
                    if (obj == null)
                    {
                        return TraceBack.ObjectNotFound;
                    }
                    IO.WriteLine(obj.ToString()??"<Unknown>");
                    return TraceBack.AllOk;
                case "nw":
                case "new":
                    if (cmdlist.Length == 1||Assembly==null) return TraceBack.InvalidCommand;
                    WorkType =
                        Assembly.GetType(cmdlist[1], false, true) ??
                        Assembly.GetType(WorkType?.FullName + '.' + cmdlist[1], false, true) ??
                        Assembly.GetType(Assembly.GetName().Name + '.' + cmdlist[1], false, true);
                    if (WorkType == null)
                    {
                        return TraceBack.ObjectNotFound;
                    }

                    if (WorkType?.FullName == null) return TraceBack.InvalidCommand;
                    WorkInstance = Assembly.CreateInstance(WorkType?.FullName??throw new NullReferenceException());
                    Prompt = (WorkType?.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute
                        ?? new MobileSuitInfoAttribute(cmdlist[1])).Prompt;
                    InstanceRef.Clear();
                    InstanceNameStk.Clear();
                    InstanceNameStk.Add($"(new {WorkType?.Name})");
                    return TraceBack.AllOk;
                case "md":
                case "modify":
                    if (WorkType == null || Assembly == null) return TraceBack.InvalidCommand;
                    if (cmdlist.Length == 1) return TraceBack.InvalidCommand;
                    else return ModifyMember(cmdlist[1..]);
                case "lv":
                case "leave":
                    if (InstanceRef.Count == 0)
                        return TraceBack.InvalidCommand;
                    if (WorkType == null) return TraceBack.InvalidCommand;
                    WorkInstance = InstanceRef.Pop();
                    InstanceNameStk.RemoveAt(InstanceNameStk.Count - 1);//PopBack
                    WorkType = WorkInstance.GetType();
                    return TraceBack.AllOk;
                case "et":
                case "enter":
                    if (cmdlist.Length == 1) return TraceBack.InvalidCommand;
                    if (WorkType == null || WorkInstance==null || Assembly == null) return TraceBack.InvalidCommand;
                    var nextobj = WorkType.GetProperty(cmdlist[1], Flags)?.GetValue(WorkInstance) ??
                        WorkType.GetField(cmdlist[1], Flags)?.GetValue(WorkInstance);
                    InstanceRef.Push(WorkInstance);
                    var fName = WorkType?.GetProperty(cmdlist[1], Flags)?.Name ??
                                WorkType?.GetField(cmdlist[1], Flags)?.Name;
                    if (fName == null || nextobj == null) return TraceBack.ObjectNotFound;
                    InstanceNameStk.Add(fName);
                    WorkInstance = nextobj;
                    WorkType = nextobj.GetType();
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
                    IO.WriteLine($"Work Instance:{WorkType.FullName}");
                    return TraceBack.AllOk;
                //case "modify":
                case "sw":
                case "switch":
                    return SwitchOption(cmdlist[1]);
                default:
                    return TraceBack.InvalidCommand;
            }

        }
        //private TraceBack ModifyValue(ref string[] cmdlist, int readindex, object? instance) { }
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
                        .Where(m => String.Equals(m.Name, cmdlist[0], StringComparison.CurrentCultureIgnoreCase))
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
                else
                {
                    mi.Invoke(instance, cmdlist[1..]);
                    return TraceBack.AllOk;
                }
            }
            else if (WorkInstance == null)
            {
                var nextobj = Assembly.GetType(cmdlist[0], false, true) ??
                    Assembly.GetType(Assembly.GetName().Name + '.' + cmdlist[0], false, true);
                if (nextobj == null)
                {
                    return TraceBack.ObjectNotFound;
                }
                else
                {
                    return RunObject(cmdlist[1..],
                        Assembly.CreateInstance(nextobj.FullName ?? throw new InvalidOperationException()));
                }
            }
            //If Null Instance
            else
            {
                return TraceBack.ObjectNotFound;

            }

        }
        public int Run(string prompt)
        {
            UpdatePrompt(prompt);
            for (; ; )
            {
                IO.Write(Prompt + '>',MobileSuitIoInterface.OutputType.Prompt);
                var cmd = IO.ReadLine();
                TraceBack tb;
                if (cmd == "")
                {
                    continue;
                }
                else if (cmd != null && cmd[0] == '@')
                {
                    tb = RunLocal(cmd.Remove(0, 1));
                }
                else
                {
                    var cmdlist = cmd?.Split(' ');
                    tb = cmdlist==null?TraceBack.InvalidCommand:RunObject(cmdlist, WorkInstance);

                }
                switch (tb)
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
                        break;
                }
                UpdatePrompt(prompt);
            }
        }

        public int Run()
            => Run("");
    }
}
