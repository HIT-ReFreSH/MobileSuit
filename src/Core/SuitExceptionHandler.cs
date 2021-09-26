using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// The handler
    /// </summary>
    public interface ISuitExceptionHandler
    {
        /// <summary>
        /// To invoke the middleware
        /// </summary>
        /// <param name="context">Context of the request.</param>
        /// <returns></returns>
        public Task InvokeAsync(SuitContext context);
        /// <summary>
        /// Get the default exception handler of MobileSuit.
        /// </summary>
        /// <returns></returns>
        public static ISuitExceptionHandler Default() => new SuitExceptionHandler();
    }
    internal class SuitExceptionHandler : ISuitExceptionHandler
    {
        public async Task InvokeAsync(SuitContext context)
        {
            if (context.Exception is null) return;
        }
    }
}
