using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{

    /// <summary>
    /// A middleware of Mobile Suit.
    /// </summary>
    public interface ISuitMiddleware
    {
        /// <summary>
        /// To invoke the middleware
        /// </summary>
        /// <param name="context">Context of the request.</param>
        /// <returns></returns>
        public Task InvokeAsync(SuitContext context);
    }
}
