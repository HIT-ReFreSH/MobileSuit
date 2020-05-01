#nullable enable
using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Members;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// Built-In-Command Server. May be Override if necessary.
    /// </summary>
    public class BuiltInCommandServer : IBuiltInCommandServer
    {
        /// <summary>
        /// Initialize a BicServer with the given SuitHost.
        /// </summary>
        /// <param name="host">The given SuitHost.</param>
        public BuiltInCommandServer(SuitHost host)
        {
            Host = host;
            HostRef = new SuitObject(Host);
        }
        /// <summary>
        /// Host
        /// </summary>
        protected SuitHost Host { get; }
        /// <summary>
        /// SuitObject for Host
        /// </summary>
        protected SuitObject HostRef { get; }
        /// <summary>
        /// Host's current SuitObject.
        /// </summary>
        protected SuitObject Current
        {
            get => Host.Current;
            set => Host.Current = value;
        }
        /// <summary>
        /// Enter a member of Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Et")]
        [SuitInfo(typeof(BicInfo), "Enter")]
        public virtual TraceBack Enter(string[] args)
        {
            if (args == null || (args.Length == 0 || Host.Assembly == null || Host.WorkType == null || Host.WorkInstance == null))
                return TraceBack.InvalidCommand;

            var r = Current.TryGetField(args[0], out var nextObject);
            if (r != TraceBack.AllOk || nextObject is null) return r;

            Host.InstanceStack.Push(Host.Current);
            var a0L = args[0].ToLower(CultureInfo.CurrentCulture);
            foreach (var name in nextObject.FriendlyNames)
                if (a0L == name.ToLower(CultureInfo.CurrentCulture))
                {
                    args[0] = name;
                    break;
                }

            Host.InstanceNameString.Add(a0L);
            Host.Current = nextObject.SuitValue;
            Host.WorkInstanceInit();
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Leave the Current SuitObject, Back to its Parent
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Lv")]
        [SuitInfo(typeof(BicInfo), "Leave")]
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
        /// Create and Enter a new SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitInfo(typeof(BicInfo), "New")]
        [SuitAlias("New")]
        public virtual TraceBack CreateObject(string[] args)
        {
            if (Host.Assembly == null || args == null) return TraceBack.InvalidCommand;

            var type =
                Host.Assembly.GetType(args[0], false, true)
                ?? Host.Assembly.GetType(Host.WorkType?.FullName + '.' + args[0], false, true)
                ?? Host.Assembly.GetType(Host.Assembly.GetName().Name + '.' + args[0], false, true);
            if (type?.FullName == null) return TraceBack.ObjectNotFound;

            Host.InstanceNameStringStack.Push(Host.InstanceNameString);
            Host.InstanceStack.Push(Host.Current);
            Host.Current = new SuitObject(Host.Assembly.CreateInstance(type.FullName));
            Host.Prompt.Update("",  
                (Host.WorkType?.GetCustomAttribute(typeof(SuitInfoAttribute)) as SuitInfoAttribute
                                                     ?? new SuitInfoAttribute(args[0])).Text,TraceBack.AllOk);
            Host.InstanceNameString.Clear();
            Host.InstanceNameString.Add($"(new {Host.WorkType?.Name})");
            Host.WorkInstanceInit();
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Show Certain Member's Value of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Vw")]
        [SuitInfo(typeof(BicInfo), "View")]
        public virtual TraceBack View(string[] args)
        {
            if (args == null || args.Length == 0 || Host.Assembly == null || Host.WorkType == null || Host.WorkInstance == null)
                return TraceBack.InvalidCommand;
            var r = Current.TryGetField(args[0], out var obj);
            if (r != TraceBack.AllOk || obj is null) return r;
            Host.IO.WriteLine(obj.ToString() ?? $"<{Lang.Unknown}>");
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Run SuitScript at the given location
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Rs")]
        [SuitInfo(typeof(BicInfo), "RunScript")]
        public virtual TraceBack RunScript(string[] args)
        {
            if (args == null || (args.Length <= 1 || !File.Exists(args[1]))) return TraceBack.InvalidCommand;
            var t = Host.RunScriptsAsync(ReadTextFileAsync(args[1]),true,Path.GetFileNameWithoutExtension(args[1]));
            t.Wait();
            return t.Result;
        }
        /// <summary>
        /// Switch Options for MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Sw")]
        [SuitInfo(typeof(BicInfo), "SwitchOption")]
        public virtual TraceBack SwitchOption(string[] args)
        {
            return ModifyValue(HostRef, args);
        }
        /// <summary>
        /// Modify Certain Member's Value of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Md")]
        [SuitInfo(typeof(BicInfo), "ModifyMember")]
        public virtual TraceBack ModifyMember(string[] args)
        {
            return ModifyValue(Current, args);
        }
        /// <summary>
        /// Show Members of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Ls")]
        [SuitInfo(typeof(BicInfo), "List")]
        public virtual TraceBack List(string[] args)
        {
            if (Host.Current == null) return TraceBack.InvalidCommand;
            Host.IO.WriteLine(Lang.Members, OutputType.ListTitle);
            ListMembers(Host.Current);
            Host.IO.WriteLine();
            Host.IO.WriteLine(IIOServer.CreateContentArray
            (
                (Lang.ViewBic, null),
                ("@Help", ConsoleColor.Cyan),
                ("'", null)
            ), OutputType.AllOk);
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Free the Current SuitObject, and back to the last one.
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Fr")]
        [SuitInfo(typeof(BicInfo), "Free")]
        public virtual TraceBack Free(string[] args)
        {
            if (Host.Current.Instance is null) return TraceBack.InvalidCommand;
            Host.Prompt.Update("",
                "", TraceBack.AllOk);
            Current = Host.InstanceStack.Count != 0
                ? Host.InstanceStack.Pop()
                : new SuitObject(null);
            Host.InstanceNameString.Clear();
            if (Host.InstanceNameStringStack.Count != 0)
                Host.InstanceNameString.AddRange(Host.InstanceNameStringStack.Pop());

            return TraceBack.AllOk;
        }
        /// <summary>
        /// Exit MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitInfo(typeof(BicInfo), "Exit")]
        [SuitAlias("Exit")]
        public virtual TraceBack ExitSuit(string[] args)
        {
            return TraceBack.OnExit;
        }
        /// <summary>
        /// Show Current SuitObject Information
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Me")]
        [SuitInfo(typeof(BicInfo), "This")]
        public virtual TraceBack This(string[] args)
        {
            if (Host.WorkType == null) return TraceBack.InvalidCommand;
            Host.IO.WriteLine($"{Lang.WorkInstance}{Host.WorkType.FullName}");
            return TraceBack.AllOk;
        }
        /// <summary>
        /// Output something in default way
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Echo")]
        [SuitInfo(typeof(BicInfo), "Print")]
        public virtual TraceBack Print(string[] args)
        {
            if (args == null || args.Length == 1)
            {
                Host.IO.WriteLine("");
                return TraceBack.AllOk;
            }

            var argumentSb = new StringBuilder();
            foreach (var arg in args[1..]) argumentSb.Append($"{arg} ");
            Host.IO.WriteLine(argumentSb.ToString()[..^1]);
            return TraceBack.AllOk;
        }
        /// <summary>
        /// A more powerful way to output something, with arg1 as option
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("EchoX")]
        [SuitInfo(typeof(BicInfo), "SuperPrint")]
        public virtual TraceBack SuperPrint(string[] args)
        {
            if (args == null || args.Length <= 2)
            {
                Host.IO.WriteLine("");
                return TraceBack.AllOk;
            }

            var argumentSb = new StringBuilder();
            foreach (var arg in args[1..]) argumentSb.Append($"{arg} ");
            Host.IO.WriteLine(argumentSb.ToString()[..^1],
                args[1].ToLower(CultureInfo.CurrentCulture) switch
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
                        c => string.Equals(c.Name.ToLower(CultureInfo.CurrentCulture), args[1].ToLower(CultureInfo.CurrentCulture),
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
        [SuitAlias("Sh")]
        [SuitInfo(typeof(BicInfo), "Shell")]
        public virtual TraceBack Shell(string[] args)
        {
            if (args == null || args.Length < 2) return TraceBack.InvalidCommand;
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
                if (!Host.UseTraceBack) throw;
                Host.IO.WriteLine($"{Lang.Error}{e}", OutputType.Error);
                return TraceBack.ObjectNotFound;
            }

            return TraceBack.AllOk;
        }
        /// <summary>
        /// Show Help of MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitInfo(typeof(BicInfo), "Help")]
        public virtual TraceBack Help(string[] args)
        {
            Host.IO.WriteLine(Lang.Bic, OutputType.ListTitle);
            ListMembers(Host.BicServer);
            Host.IO.WriteLine();
            Host.IO.WriteLine(IIOServer.CreateContentArray
            (
                (Lang.BicExp1, null),
                ("@", ConsoleColor.Cyan),
                (Lang.BicExp2,
                    null)
            ), OutputType.AllOk);
            return TraceBack.AllOk;
        }
        /// <summary>
        /// List members of a SuitObject
        /// </summary>
        /// <param name="suitObject">The SuitObject, Maybe this BicServer.</param>
        protected void ListMembers(SuitObject suitObject)
        {
            if (suitObject == null) return;
            Host.IO.AppendWriteLinePrefix();

            foreach (var (name, member) in suitObject)
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
                Host.IO.WriteLine(IIOServer.CreateContentArray
                (
                    (name, null),
                    (aliasesExpression.ToString(), ConsoleColor.DarkYellow),
                    ($" {lChar}{member.Information}{rChar}", infoColor)
                ));
            }

            Host.IO.SubtractWriteLinePrefix();
        }
        /// <summary>
        /// Asynchronously read all lines in a text file into a IAsyncEnumerable
        /// </summary>
        /// <param name="fileName">The file's name.</param>
        /// <returns>The file's content</returns>
        protected static async IAsyncEnumerable<string?> ReadTextFileAsync(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists) throw new FileNotFoundException(fileName);
            var reader = fileInfo.OpenText();
            for (; ; )
            {
                var r = await reader.ReadLineAsync().ConfigureAwait(false);
                yield return r;
                if (r is null) break;
            }

        }
        /// <summary>
        /// Modify member's value of a SuitObject
        /// </summary>
        /// <param name="suitObject">the SuitObject, maybe SuitHost.</param>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        protected TraceBack ModifyValue(SuitObject suitObject, string[] args)
        {
            if (args == null || args.Length == 0 || suitObject == null) return TraceBack.InvalidCommand;
            var r = suitObject.TryGetField(args[0], out var target);
            if (r != TraceBack.AllOk || target?.Value is null) return r;
            string val, newVal;
            if (args.Length == 1)
            {
                if (target.ValueType != typeof(bool)) return TraceBack.InvalidCommand;
                var currentBool = (bool)target.Value;
                val = currentBool.ToString(CultureInfo.CurrentCulture);
                currentBool = !currentBool;
                newVal = currentBool.ToString(CultureInfo.CurrentCulture);
                target.Value = currentBool;
            }
            else if (target.ValueType == typeof(bool))
            {
                if (target.ValueType != typeof(bool)) return TraceBack.InvalidCommand;
                var currentBool = (bool)target.Value;
                val = currentBool.ToString(CultureInfo.CurrentCulture);
                var setV = args[1].ToLower(CultureInfo.CurrentCulture);
                currentBool = setV switch
                {
                    { } when setV[0] == 't' => true,
                    _ => false
                };
                newVal = currentBool.ToString(CultureInfo.CurrentCulture);
                target.Value = currentBool;
            }
            else
            {
                val = target.Value.ToString() ?? $"<{Lang.Unknown}>";
                target.Value = (target.Converter ?? (a => a))(args[1]);
                newVal = args[1];
            }

            var a0L = args[0].ToLower(CultureInfo.CurrentCulture);
            foreach (var name in target.FriendlyNames)
                if (a0L == name.ToLower(CultureInfo.CurrentCulture))
                {
                    args[0] = name;
                    break;
                }

            Host.IO.WriteLine($"{a0L}:{val}->{newVal}", ConsoleColor.Green);
            return TraceBack.AllOk;
        }
    }
}