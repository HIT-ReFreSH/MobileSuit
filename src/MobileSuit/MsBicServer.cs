#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;
using PlasticMetal.MobileSuit.ObjectModel.Members;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// Built-In-Command Server. May be Override if necessary.
    /// </summary>
    public class MsBicServer : IMsBicServer
    {
        /// <summary>
        /// Initialize a BicServer with the given MsHost.
        /// </summary>
        /// <param name="host">The given MsHost.</param>
        public MsBicServer(MsHost host)
        {
            Host = host;
            HostRef = new MsObject(Host);
        }
        /// <summary>
        /// Host
        /// </summary>
        protected MsHost Host { get; }
        /// <summary>
        /// MsObject for Host
        /// </summary>
        protected MsObject HostRef { get; }
        /// <summary>
        /// Host's current MsObject.
        /// </summary>
        protected MsObject Current
        {
            get => Host.Current;
            set => Host.Current = value;
        }
        /// <summary>
        /// Enter a member of Current MsObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Et")]
        [MsInfo("Enter a member of Current MsObject")]
        public virtual TraceBack Enter(string[] args)
        {
            if (args.Length == 0 || Host.Assembly == null || Host.WorkType == null || Host.WorkInstance == null)
                return TraceBack.InvalidCommand;
            var r = Current.TryGetField(args[0], out var nextObject);
            if (r != TraceBack.AllOk || nextObject is null) return r;
            Host.InstanceStack.Push(Host.Current);
            var a0L = args[0].ToLower();
            foreach (var name in nextObject.FriendlyNames)
                if (a0L == name.ToLower())
                {
                    args[0] = name;
                    break;
                }

            Host.InstanceNameString.Add(a0L);
            Host.Current = nextObject.MsValue;
            Host.WorkInstanceInit();
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Leave the Current MsObject, Back to its Parent
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Lv")]
        [MsInfo("Leave the Current MsObject, Back to its Parent")]
        public virtual TraceBack Leave(string[] args)
        {
            if (Host.InstanceStack.Count == 0 || Current is null)
                return TraceBack.InvalidCommand;
            Host.Current = Host.InstanceStack.Pop();
            Host.InstanceNameString.RemoveAt(Host.InstanceNameString.Count - 1); //PopBack
            Host.WorkInstanceInit();
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Create and Enter a new MsObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsInfo("Create and Enter a new MsObject")]
        public virtual TraceBack New(string[] args)
        {
            if (Host.Assembly == null) return TraceBack.InvalidCommand;

            var type =
                Host.Assembly.GetType(args[0], false, true)
                ?? Host.Assembly.GetType(Host.WorkType?.FullName + '.' + args[0], false, true)
                ?? Host.Assembly.GetType(Host.Assembly.GetName().Name + '.' + args[0], false, true);
            if (type?.FullName == null) return TraceBack.ObjectNotFound;

            Host.InstanceNameStringStack.Push(Host.InstanceNameString);
            Host.InstanceStack.Push(Host.Current);
            Host.Current = new MsObject(Host.Assembly.CreateInstance(type.FullName));
            Host.Prompt = (Host.WorkType?.GetCustomAttribute(typeof(MsInfoAttribute)) as MsInfoAttribute
                           ?? new MsInfoAttribute(args[0])).Text;
            Host.InstanceNameString = new List<string> { $"(new {Host.WorkType?.Name})" };
            Host.WorkInstanceInit();
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Show Certain Member's Value of the Current MsObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Vw")]
        [MsInfo("Show Certain Member's Value of the Current MsObject")]
        public virtual TraceBack View(string[] args)
        {
            if (args.Length == 0 || Host.Assembly == null || Host.WorkType == null || Host.WorkInstance == null)
                return TraceBack.InvalidCommand;
            var r = Current.TryGetField(args[0], out var obj);
            if (r != TraceBack.AllOk || obj is null) return r;
            Host.Io.WriteLine(obj.ToString() ?? "<Unknown>");
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Run MsScript at the given location
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Rs")]
        [MsInfo("Run MsScript at the given location")]
        public virtual TraceBack RunScript(string[] args)
        {
            if (args.Length <= 1 || !File.Exists(args[1])) return TraceBack.InvalidCommand;
            var t = Host.RunScriptsAsync(ReadTextFileAsync(args[1]));
            t.Wait();
            return t.Result;
        }
        /// <summary>
        /// Switch Options for MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Sw")]
        [MsInfo("Switch Options for MobileSuit")]
        public virtual TraceBack SwitchOption(string[] args)
        {
            return ModifyValue(HostRef, args);
        }
        /// <summary>
        /// Modify Certain Member's Value of the Current MsObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Md")]
        [MsInfo("Modify Certain Member's Value of the Current MsObject")]
        public virtual TraceBack ModifyMember(string[] args)
        {
            return ModifyValue(Current, args);
        }
        /// <summary>
        /// Show Members of the Current MsObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Ls")]
        [MsInfo("Show Members of the Current MsObject")]
        public virtual TraceBack List(string[] args)
        {
            if (Host.Current == null) return TraceBack.InvalidCommand;
            Host.Io.WriteLine("Members:", OutputType.ListTitle);
            ListMembers(Host.Current);
            Host.Io.WriteLine();
            Host.Io.WriteLine(new (string, ConsoleColor?)[]
            {
                ("To View Built-In Commands, use Command '", null),
                ("@Help", ConsoleColor.Cyan),
                ("'", null)
            }, OutputType.AllOk);
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Free the Current MsObject, and back to the last one.
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Fr")]
        [MsInfo("Free the Current MsObject, and back to the last one.")]
        public virtual TraceBack Free(string[] args)
        {
            if (Host.Current.Instance is null) return TraceBack.InvalidCommand;
            Host.Prompt = "";
            Current = Host.InstanceStack.Count != 0
                ? Host.InstanceStack.Pop()
                : new MsObject(null);
            Host.InstanceNameString = Host.InstanceNameStringStack.Count != 0
                ? Host.InstanceNameStringStack.Pop()
                : new List<string>();
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Exit MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsInfo("Exit MobileSuit")]
        public virtual TraceBack Exit(string[] args)
        {
            return TraceBack.OnExit;
        }
        /// <summary>
        /// Show Current MsObject Information
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Me")]
        [MsInfo("Show Current MsObject Information")]
        public virtual TraceBack This(string[] args)
        {
            if (Host.WorkType == null) return TraceBack.InvalidCommand;
            Host.Io.WriteLine($"Work Instance:{Host.WorkType.FullName}");
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Output something in default way
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Echo")]
        [MsInfo("Output something in default way")]
        public virtual TraceBack Print(string[] args)
        {
            if (args.Length == 1)
            {
                Host.Io.WriteLine("");
                return TraceBack.AllOk;
            }

            var argumentSb = new StringBuilder();
            foreach (var arg in args[1..]) argumentSb.Append($"{arg} ");
            Host.Io.WriteLine(argumentSb.ToString()[..^1]);
            return TraceBack.AllOk;
        }
        /// <summary>
        /// A more powerful way to output something, with arg1 as option
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("EchoX")]
        [MsInfo("A more powerful way to output something, with arg1 as option")]
        public virtual TraceBack SuperPrint(string[] args)
        {
            if (args.Length <= 2)
            {
                Host.Io.WriteLine("");
                return TraceBack.AllOk;
            }

            var argumentSb = new StringBuilder();
            foreach (var arg in args[1..]) argumentSb.Append($"{arg} ");
            Host.Io.WriteLine(argumentSb.ToString()[..^1],
                args[1].ToLower() switch
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
                typeof(ConsoleColor).GetFields(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(
                        c => string.Equals(c.Name.ToLower(), args[1].ToLower(),
                            StringComparison.CurrentCultureIgnoreCase))
                    ?.GetValue(null) as ConsoleColor?
            );
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Execute command with the System Shell
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsAlias("Sh")]
        [MsInfo("Execute command with the System Shell")]
        public virtual TraceBack Shell(string[] args)
        {
            if (args.Length < 2) return TraceBack.InvalidCommand;
            var proc = new Process { StartInfo = { UseShellExecute = true, FileName = args[1] } };
            if (args.Length > 2)
            {
                var argumentSb = new StringBuilder();
                foreach (var arg in args[2..]) argumentSb.Append($"{arg} ");

                proc.StartInfo.Arguments = argumentSb.ToString()[..^1];
            }

            try
            {
                proc.Start();
            }
            catch (Exception e)
            {
                Host.Io.WriteLine($"Error:{e}", OutputType.Error);
                return TraceBack.ObjectNotFound;
            }

            return TraceBack.AllOk;
        }
        /// <summary>
        /// Show Help of MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [MsInfo("Show Help of MobileSuit")]
        public virtual TraceBack Help(string[] args)
        {
            Host.Io.WriteLine("Built-In Commands:", OutputType.ListTitle);
            ListMembers(Host.BicServer);
            Host.Io.WriteLine();
            Host.Io.WriteLine(new (string, ConsoleColor?)[]
            {
                ("All of the Built-In Commands Can be used as default with the starting'", null),
                ("@", ConsoleColor.Cyan),
                ("'; However, if the object do not contains commands which have the same name as a certain Built-In Command, '@' is not necessary.",
                    null)
            }, OutputType.AllOk);
            return TraceBack.AllOk;
        }

        protected void ListMembers(MsObject obj)
        {
            Host.Io.AppendWriteLinePrefix();

            foreach (var (name, member) in obj)
            {
                var (infoColor, lChar, rChar) = member.Type switch
                {
                    MemberType.MethodWithInfo => (ConsoleColor.Blue, '[', ']'),
                    MemberType.MethodWithoutInfo => (ConsoleColor.DarkBlue, '(', ')'),
                    MemberType.FieldWithInfo => (ConsoleColor.Green, '<', '>'),
                    _ => (ConsoleColor.DarkGreen, '{', '}')
                };
                var aliasesExpression = new StringBuilder();
                foreach (var alias in member.Aliases) aliasesExpression.Append($"/{alias}");
                Host.Io.WriteLine(new (string, ConsoleColor?)[]
                {
                    (name, null),
                    (aliasesExpression.ToString(), ConsoleColor.DarkYellow),
                    ($"{lChar}{member.Information}{rChar}", infoColor)
                });
            }

            Host.Io.SubtractWriteLinePrefix();
        }

        protected async IAsyncEnumerable<string?> ReadTextFileAsync(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists) throw new FileNotFoundException(fileName);
            var reader = fileInfo.OpenText();
            for (; ; )
                yield return await reader.ReadLineAsync();
        }

        protected TraceBack ModifyValue(MsObject obj, string[] args)
        {
            if (args.Length == 0) return TraceBack.InvalidCommand;
            var r = obj.TryGetField(args[0], out var target);
            if (r != TraceBack.AllOk || target?.Value is null) return r;
            string val, newVal;
            if (args.Length == 1)
            {
                if (target.ValueType != typeof(bool)) return TraceBack.InvalidCommand;
                var currentBool = (bool)target.Value;
                val = currentBool.ToString();
                currentBool = !currentBool;
                newVal = currentBool.ToString();
                target.Value = currentBool;
            }
            else if (target.ValueType == typeof(bool))
            {
                if (target.ValueType != typeof(bool)) return TraceBack.InvalidCommand;
                var currentBool = (bool)target.Value;
                val = currentBool.ToString();
                var setV = args[1].ToLower();
                currentBool = setV switch
                {
                    { } when setV[0] == 't' => true,
                    _ => false
                };
                newVal = currentBool.ToString();
                target.Value = currentBool;
            }
            else
            {
                val = target.Value.ToString() ?? "<Unknown>";
                target.Value = (target.Converter ?? (a => a))(args[1]);
                newVal = args[1];
            }

            var a0L = args[0].ToLower();
            foreach (var name in target.FriendlyNames)
                if (a0L == name.ToLower())
                {
                    args[0] = name;
                    break;
                }

            Host.Io.WriteLine($"{a0L}:{val}->{newVal}", ConsoleColor.Green);
            return TraceBack.AllOk;
        }
    }
}