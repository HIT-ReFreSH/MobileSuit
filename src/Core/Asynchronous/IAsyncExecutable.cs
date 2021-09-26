using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core.Asynchronous
{
    /// <summary>
    ///     Represents an entity which can be executed.
    /// </summary>
    public interface IAsyncExecutable:IExecutable
    {
        /// <summary>
        ///     Execute this object.
        /// </summary>
        /// <param name="args">The arguments for execution.</param>
        /// <param name="token"></param>
        /// <param name="returnValue">the return value of execute.</param>
        /// <returns>TraceBack result of this object.</returns>
        public Task<RequestStatus> ExecuteAsync(string[] args, CancellationToken token,out object? returnValue);
    }
}
