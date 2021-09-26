using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// Context through the lifetime of a SuitRequest(A Command)
    /// </summary>
    public class SuitContext:IDisposable
    {
        private readonly IServiceScope _serviceScope;
        /// <summary>
        /// CancellationToken of the request.
        /// </summary>
        public CancellationToken CancellationToken { get;  }
        /// <summary>
        /// Properties of current request.
        /// </summary>
        public Dictionary<string, string> Properties { get;  } = new();
        /// <summary>
        /// The execution status of current request.
        /// </summary>
        public RequestStatus Status { get; set; } = RequestStatus.NoRequest;
        /// <summary>
        /// The exception caught in the execution.
        /// </summary>
        public Exception? Exception { get; set; }
        /// <summary>
        /// A command from input stream
        /// </summary>
        public string[] Request { get; set; } = Array.Empty<string>();
        /// <summary>
        /// The ServiceProvider who provides services through whole request.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        internal SuitContext(IServiceScope scope, CancellationToken token)
        {
            _serviceScope = scope;
            ServiceProvider = scope.ServiceProvider;
            CancellationToken = token;
        }
        /// <summary>
        /// Output to the output stream
        /// </summary>
        public string? Response { get; set; }
        /// <inheritdoc/>
        public void Dispose()
        {
            this._serviceScope.Dispose();
        }
    }
}
