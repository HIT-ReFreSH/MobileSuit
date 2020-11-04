using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// When exiting, Call OnExit() of the object which implements this interface. 
    /// </summary>
    public interface IExitInteractive
    {
        /// <summary>
        /// The Method to run. When leaving SuitHost.Run().
        /// </summary>
        public void OnExit();
    }
}
