using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     A host of Mobile Suit, which may run commands.
    /// </summary>
    public interface IMobileSuitHost:IDisposable
    {
        /// <summary>
        /// Start A mobile suit
        /// </summary>
        /// <param name="cancellationToken"></param>
        public Task StartAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Stop A mobile suit
        /// </summary>
        /// <param name="cancellationToken"></param>
        public Task StopAsync(CancellationToken cancellationToken);
    }

}