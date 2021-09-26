using System.Threading;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Represents an entity which can be executed.
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        ///     Execute this object.
        /// </summary>
        /// <param name="args">The arguments for execution.</param>
        /// <param name="token"></param>
        /// <returns>Result of executing this object.</returns>
        public  Task<ExecuteResult> Execute(string[] args, CancellationToken token);
    }
}