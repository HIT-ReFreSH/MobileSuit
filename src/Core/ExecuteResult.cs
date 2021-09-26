using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// Return value and TraceBack for a command.
    /// </summary>
    public record ExecuteResult
    {
        /// <summary>
        /// Return value of command. NULL if not exist, Exception if exception happened.
        /// </summary>
        public object? ReturnValue { get; init; }
        /// <summary>
        /// TraceBack of command.
        /// </summary>
        public RequestStatus TraceBack { get; init; }
    }
}
