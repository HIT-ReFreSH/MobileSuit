using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// Builder to configurate mobile suit app.
    /// </summary>
    public interface ISuitAppBuilder {
        /// <summary>
        /// Add a layer of Middleware of Suit.
        /// </summary>
        /// <param name="middleware"></param>
        public void UseMiddleware(ISuitMiddleware middleware);
        /// <summary>
        /// Middlewares embedded.
        /// </summary>
        public IEnumerable<ISuitMiddleware> Middlewares { get; }
        /// <summary>
        /// Lock the middleware list.
        /// </summary>
        public void Build();
    }
    /// <summary>
    /// Raw implement of ISuitAppBuilder
    /// </summary>
    public class SuitAppBuilder : ISuitAppBuilder
    {
        private bool _lock = false;
        private readonly List<ISuitMiddleware> _middlewares = new();
        /// <inheritdoc/>
        public void UseMiddleware(ISuitMiddleware middleware) { if (!_lock) _middlewares.Add(middleware); }
        /// <inheritdoc/>
        public virtual void Build()
        {
            _lock = true;
        }

        /// <inheritdoc/>
        public IEnumerable<ISuitMiddleware> Middlewares => _middlewares;
    }
    internal class DefaultSuitAppBuilder
    {

    }
}
