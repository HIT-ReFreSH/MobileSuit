using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// has access to host's logger
    /// </summary>
    public interface ILogInteractive
    {
        /// <summary>
        ///     Provides Interface for SuitHost to set Logger
        /// </summary>
        [SuitIgnore]
        IAssignOnceLogger Log { get; }
    }
}
