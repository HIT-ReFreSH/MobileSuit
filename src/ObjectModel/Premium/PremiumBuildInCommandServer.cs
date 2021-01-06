#nullable enable
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel.Premium
{
    /// <summary>
    ///     Built-In-Command Server. May be Override if necessary.
    /// </summary>
    public class PremiumBuildInCommandServer : BuildInCommandServer, IAdvancedBuildInCommandServer,
        IDynamicBuildInCommandServer
    {
        /// <summary>
        ///     Initialize a BicServer with the given SuitHost.
        /// </summary>
        /// <param name="host">The given SuitHost.</param>
        public PremiumBuildInCommandServer(SuitHost host) : base(host)
        {
        }


        /// <summary>
        ///     Switch Options for MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Sw")]
        [SuitInfo(typeof(BuildInCommandInformations), "SwitchOption")]
        public virtual TraceBack SwitchOption(string[] args)
        {
            return ModifyValue(HostRef, args);
        }


        /// <summary>
        ///     Output something in default way
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Echo")]
        [SuitInfo(typeof(BuildInCommandInformations), "Print")]
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
        ///     A more powerful way to output something, with arg1 as option
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("EchoX")]
        [SuitInfo(typeof(BuildInCommandInformations), "SuperPrint")]
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
                        c => string.Equals(c.Name.ToLower(CultureInfo.CurrentCulture),
                            args[1].ToLower(CultureInfo.CurrentCulture),
                            StringComparison.CurrentCultureIgnoreCase))
                    ?.GetValue(null) as ConsoleColor?
            );
            return TraceBack.AllOk;
        }

        /// <summary>
        ///     Execute command with the System Shell
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Sh")]
        [SuitInfo(typeof(BuildInCommandInformations), "Shell")]
        public virtual TraceBack Shell(string[] args)
        {
            if (args == null || args.Length < 2) return TraceBack.InvalidCommand;
            var proc = new Process {StartInfo = {UseShellExecute = true, FileName = args[1]}};
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
                if (Host.Settings.EnableThrows) throw;
                Host.IO.WriteLine($"{Lang.Error}{e}", OutputType.Error);
                return TraceBack.ObjectNotFound;
            }

            return TraceBack.AllOk;
        }

        /// <summary>
        ///     Create and Enter a new SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitInfo(typeof(BuildInCommandInformations), "New")]
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
                 ?? new SuitInfoAttribute(args[0])).Text, TraceBack.AllOk);
            Host.InstanceNameString.Clear();
            Host.InstanceNameString.Add($"(new {Host.WorkType?.Name})");
            Host.WorkInstanceInit();
            return TraceBack.AllOk;
        }


        /// <summary>
        ///     Free the Current SuitObject, and back to the last one.
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Fr")]
        [SuitInfo(typeof(BuildInCommandInformations), "Free")]
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
    }
}