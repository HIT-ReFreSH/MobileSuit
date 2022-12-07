using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PlasticMetal.MobileSuit.SuitUtils;

namespace PlasticMetal.MobileSuit.Core.Services;

/// <summary>
///     Built-In-Command Server. May be Override if necessary.
/// </summary>
public class SuitCommandServer : ISuitCommandServer
{
    /// <summary>
    ///     Initialize a BicServer with the given SuitHost.
    /// </summary>
    public SuitCommandServer(IIOHub io, SuitAppShell app, SuitHostShell host, ITaskService taskService)
    {
        IO = io;
        App = app;
        Host = host;
        TaskService = taskService;
    }

    private IIOHub IO { get; }
    private SuitAppShell App { get; }
    private SuitHostShell Host { get; }
    private ITaskService TaskService { get; }


    /// <inheritdoc />
    [SuitAlias("Ls")]
    [SuitInfo(typeof(BuildInCommandInformations), "List")]
    public virtual async Task ListCommands(string[] args)
    {
        await IO.WriteLineAsync(Lang.Members, OutputType.Title);
        await ListMembersAsync(App);
        await IO.WriteLineAsync();
        await IO.WriteLineAsync(CreateContentArray
        (
            (Lang.ViewBic, null),
            ("@Help", ConsoleColor.Cyan),
            ("'", null)
        ), OutputType.Ok);
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), "Exit")]
    [SuitAlias("Exit")]
    public virtual RequestStatus ExitSuit()
    {
        return RequestStatus.OnExit;
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), nameof(Join))]
    public async Task<string?> Join(int index, [SuitInjected] SuitContext context)
    {
        if (TaskService.HasTask(index))
        {
            await TaskService.Join(index, context);
            return context.Response;
        }

        await IO.WriteLineAsync(BuildInCommandInformations.TaskNotFound, OutputType.Error);
        return null;
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), nameof(Tasks))]
    public async Task Tasks()
    {
        await IO.WriteLineAsync(Regex.Unescape(BuildInCommandInformations.Tasks_Title), OutputType.Title);
        foreach (var task in TaskService.GetTasks())
        {
            var line = new List<PrintUnit>
            {
                (task.Index.ToString(), null, null),
                ("\t", null, null),
                (task.Request, IO.ColorSetting.InformationColor, null),
                ("\t", null, null),
                task.Status switch
                {
                    RequestStatus.Ok or RequestStatus.NoRequest or RequestStatus.Handled => (Lang.Done,
                        IO.ColorSetting.OkColor),
                    RequestStatus.Running => (Lang.Running, IO.ColorSetting.WarningColor),
                    RequestStatus.CommandParsingFailure => (Lang.InvalidCommand, IO.ColorSetting.ErrorColor),
                    RequestStatus.CommandNotFound => (Lang.MemberNotFound, IO.ColorSetting.ErrorColor),
                    RequestStatus.Interrupt => (Lang.Interrupt, IO.ColorSetting.ErrorColor),
                    _ => (Lang.OnError, IO.ColorSetting.ErrorColor)
                },
                ("\t", null, null),
                (task.Response ?? "-", IO.ColorSetting.InformationColor, null)
            };
            await IO.WriteLineAsync(line);
        }
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), nameof(Stop))]
    public async Task Stop(int index)
    {
        if (TaskService.HasTask(index))
            TaskService.Stop(index);
        else
            await IO.WriteLineAsync(BuildInCommandInformations.TaskNotFound, OutputType.Error);
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), nameof(ClearCompleted))]
    [SuitAlias("Cct")]
    public async Task ClearCompleted()
    {
        TaskService.ClearCompleted();
        await Tasks();
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), nameof(Dir))]
    public string Dir()
    {
        return Directory.GetCurrentDirectory();
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), nameof(ChDir))]
    [SuitAlias("cd")]
    public string ChDir(string path)
    {
        if (!Directory.Exists(path)) return BuildInCommandInformations.DirectoryNotFound;
        Directory.SetCurrentDirectory(path);
        return Directory.GetCurrentDirectory();
    }

    /// <inheritdoc />
    [SuitInfo(typeof(BuildInCommandInformations), nameof(Help))]
    public virtual async Task Help(string[] args)
    {
        await IO.WriteLineAsync(Lang.Bic, OutputType.Title);
        await ListMembersAsync(Host);
        await IO.WriteLineAsync();
        await IO.WriteLineAsync(CreateContentArray
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
            await IO.WriteLineAsync(CreateContentArray
            (
                (shell.AbsoluteName, null),
                (aliasesExpression.ToString(), ConsoleColor.DarkYellow),
                ($" {lChar}{shell.Information}{rChar}", infoColor)
            ));
        }

        IO.SubtractWriteLinePrefix();
    }
}