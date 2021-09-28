using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core.Services
{
    /// <summary>
    /// Provides task system.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Number of Tasks that are running.
        /// </summary>
        public int RunningCount{ get; }

        void AddTask(Task task, SuitContext context);
    }
    internal class TaskService:ITaskService
    {
        /// <inheritdoc/>
        public int RunningCount { get; } = 0;
    }
}
