using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// After initialized, Call OnInitialized() of the object which implements this interface.
    /// </summary>
    public interface IStartingInteractive
    {
        /// <summary>
        /// The Method to run.
        /// </summary>
        public void OnInitialized();
    }
}
