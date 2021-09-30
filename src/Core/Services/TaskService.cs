using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PlasticMetal.MobileSuit.Core.Services
{
    /// <summary>
    /// Information of Suit Task
    /// </summary>
    public record TaskInfo
    {
        /// <summary>
        /// Index of Task
        /// </summary>
        public int Index { get; init; }
        /// <summary>
        /// Status of Request
        /// </summary>
        public RequestStatus Status { get; init; }

        /// <summary>
        /// Response or exception
        /// </summary>
        public string? Response { get; init; } = null;
        /// <summary>
        /// Request
        /// </summary>
        public string Request { get; init; } = string.Empty;
    }
    /// <summary>
    /// Provides task system.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Has a task with given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasTask(int index);
        /// <summary>
        /// Number of Tasks that are running.
        /// </summary>
        public int RunningCount { get; }
        /// <summary>
        /// Add a task to Task Collection
        /// </summary>
        /// <param name="task"></param>
        /// <param name="context"></param>
        public void AddTask(Task task, SuitContext context);
        /// <summary>
        /// Get All tasks in Collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskInfo> GetTasks();
        /// <summary>
        /// Stop the task with certain index.
        /// </summary>
        /// <param name="index"></param>
        public void Stop(int index);

        /// <summary>
        /// Join the task with certain index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newContext"></param>
        public Task Join(int index, SuitContext newContext);
        /// <summary>
        /// Remove the completed tasks.
        /// </summary>
        public void ClearCompleted();
    }
    internal class TaskService : ITaskService
    {
        private readonly List<(Task, SuitContext)> _tasks = new();

        /// <inheritdoc/>
        public bool HasTask(int index)
            => _tasks.Count > index && index >= 0;

        /// <inheritdoc/>
        public int RunningCount => _tasks.Count(t => t.Item2.Status == RequestStatus.Running);
        /// <inheritdoc/>
        public void AddTask(Task task, SuitContext context)
        {
            var io = context.ServiceProvider.GetRequiredService<IIOHub>();
            io.AppendWriteLinePrefix((PrintUnit)(_tasks.Count.ToString(), io.ColorSetting.SystemColor, null));
            _tasks.Add((task, context));
        }

        public IEnumerable<TaskInfo> GetTasks()
        {
            var i = 0;
            foreach (var (_, context) in _tasks)
            {
                yield return new()
                {
                    Index = i++,
                    Request = string.Join(' ', context.Request),
                    Response = context.Response,
                    Status = context.Status
                };
            }
        }

        public void Stop(int index)
        {
            _tasks[index].Item2.CancellationToken.Cancel();
        }

        public async Task Join(int index, SuitContext newContext)
        {
            var (task, context) = _tasks[index];
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
}
