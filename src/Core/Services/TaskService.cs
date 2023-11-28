using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     Information of Suit Task
/// </summary>
public record TaskInfo
{
    /// <summary>
    ///     Index of Task
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    ///     Status of Request
    /// </summary>
    public RequestStatus Status { get; init; }

    /// <summary>
    ///     Response or exception
    /// </summary>
    public string? Response { get; init; }

    /// <summary>
    ///     Request
    /// </summary>
    public string Request { get; init; } = string.Empty;
}

/// <summary>
///     Provides task system.
/// </summary>
public interface ITaskService
{
    /// <summary>
    ///     Number of Tasks that are running.
    /// </summary>
    public int RunningCount { get; }

    /// <summary>
    ///     Has a task with given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool HasTask(int index);

    /// <summary>
    ///     Add a task to Task Collection
    /// </summary>
    /// <param name="task"></param>
    /// <param name="context"></param>
    public void AddTask(Task task, SuitContext context);

    /// <summary>
    ///     Get All tasks in Collection.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TaskInfo> GetTasks();
    /// <summary>
    /// Run some task immediately
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public ValueTask RunTaskImmediately(Task task);
    /// <summary>
    ///     Stop the task with certain index.
    /// </summary>
    /// <param name="index"></param>
    public void Stop(int index);

    /// <summary>
    ///     Join the task with certain index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="newContext"></param>
    public Task Join(int index, SuitContext newContext);

    /// <summary>
    ///     Remove the completed tasks.
    /// </summary>
    public void ClearCompleted();
}

internal class TaskRecorder : IEnumerable<Task>
{
    public bool IsLocked { get; set; }
    private List<Task> _tasks = new();
    public IEnumerator<Task> GetEnumerator() => _tasks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public bool Add(Task task)
    {
        if (!IsLocked) _tasks.Add(task);
        return !IsLocked;
    }
    public bool Remove(Task task)
    {
        if (!IsLocked) _tasks.Remove(task);
        return !IsLocked;
    }
}
internal class TaskService : ITaskService
{
    private readonly TaskRecorder _cancelTasks;
    private readonly List<(Task, SuitContext)> _tasks = new();

    public TaskService(TaskRecorder cancelTasks)
    {
        _cancelTasks = cancelTasks;
    }

    /// <inheritdoc />
    public bool HasTask(int index)
    {
        return _tasks.Count > index && index >= 0;
    }

    /// <inheritdoc />
    public int RunningCount => _tasks.Count(t => t.Item2.Status == RequestStatus.Running);

    /// <inheritdoc />
    public void AddTask(Task task, SuitContext context)
    {
        var io = context.ServiceProvider.GetRequiredService<IIOHub>();
        io.AppendWriteLinePrefix((_tasks.Count.ToString(), io.ColorSetting.SystemColor, null));
        _tasks.Add((task, context));
        _cancelTasks.Add(task);
    }

    public IEnumerable<TaskInfo> GetTasks()
    {
        var i = 0;
        foreach (var (_, context) in _tasks)
            yield return new TaskInfo
            {
                Index = i++,
                Request = string.Join(' ', context.Request),
                Response = context.Response,
                Status = context.Status
            };
    }

    public async ValueTask RunTaskImmediately(Task task)
    {
        _cancelTasks.Add(task);
        await task;
        _cancelTasks.Remove(task);
    }

    public void Stop(int index)
    {
        var (task, context) = _tasks[index];
        context.CancellationToken.Cancel();
        _cancelTasks.Remove(task);
    }

    public async Task Join(int index, SuitContext newContext)
    {
        var (task, context) = _tasks[index];
        _cancelTasks.Remove(task);
        await task;
        newContext.Status = context.Status;
        newContext.Response = context.Response;
    }

    public void ClearCompleted()
    {
        for (var i = _tasks.Count - 1; i >= 0; i--)
        {
            if (_tasks[i].Item2.Status == RequestStatus.Running) continue;
            _tasks[i].Item2.Dispose();
            _tasks.RemoveAt(i);
        }
    }
}