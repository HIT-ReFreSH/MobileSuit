using System;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core.Services
{
    /// <summary>
    ///     Built-In-Command Server. May be Override if necessary.
    /// </summary>
    public class SuitCommandServer : ISuitCommandServer
    {
        public IIOHub IO { get; }
        public SuitAppShell App { get; }
        public SuitHostShell Host { get; }

        /// <summary>
        ///     Initialize a BicServer with the given SuitHost.
        /// </summary>
        public SuitCommandServer(IIOHub io, SuitAppShell app,SuitHostShell host)
        {
            IO = io;
            App = app;
            Host = host;
        }




        /// <summary>
        ///     Show Members of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitAlias("Ls")]
        [SuitInfo(typeof(BuildInCommandInformations), "List")]
        public virtual async Task ListCommands(string[] args)
        {
            await IO.WriteLineAsync(Lang.Members, OutputType.Title);
            await ListMembersAsync(App);
            await IO.WriteLineAsync();
            await IO.WriteLineAsync(SuitTools.CreateContentArray
            (
                (Lang.ViewBic, null),
                ("@Help", ConsoleColor.Cyan),
                ("'", null)
            ), OutputType.Ok);
        }
        /// <summary>
        ///     Exit MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitInfo(typeof(BuildInCommandInformations), "Exit")]
        [SuitAlias("Exit")]
        public virtual async Task ExitSuit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Show Help of MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        [SuitInfo(typeof(BuildInCommandInformations), "Help")]
        public virtual async Task Help(string[] args)
        {
            await IO.WriteLineAsync(Lang.Bic, OutputType.Title);
            await ListMembersAsync(Host);
            await IO.WriteLineAsync();
            await IO.WriteLineAsync(SuitTools.CreateContentArray
            (
                (Lang.BicExp1, null),
                ("@", ConsoleColor.Cyan),
                (Lang.BicExp2,
                    null)
            ), OutputType.Ok);
        }

        /// <summary>
        ///     List members of a SuitObject
        /// </summary>
        /// <param name="suitObject">The SuitObject, Maybe this BicServer.</param>
        protected async Task ListMembersAsync(ISuitShellCollection suitObject)
        {
            if (suitObject == null) return;
            IO.AppendWriteLinePrefix();

            foreach (var shell in suitObject.Members())
            {
                var (infoColor, lChar, rChar) = shell.Type switch
                {
                    MemberType.MethodWithInfo => (ConsoleColor.Blue, '[', ']'),
                    MemberType.MethodWithoutInfo => (ConsoleColor.DarkBlue, '(', ')'),
                    MemberType.FieldWithInfo => (ConsoleColor.Green, '<', '>'),
                    _ => (ConsoleColor.DarkGreen, '{', '}')
                };
                var aliasesExpression = new StringBuilder();
                foreach (var alias in shell.Aliases) aliasesExpression.Append($"/{alias}");
                await IO.WriteLineAsync(SuitTools.CreateContentArray
                (
                    (shell.AbsoluteName, null),
                    (aliasesExpression.ToString(), ConsoleColor.DarkYellow),
                    ($" {lChar}{shell.Information}{rChar}", infoColor)
                ));
            }

            IO.SubtractWriteLinePrefix();
        }



    }
}