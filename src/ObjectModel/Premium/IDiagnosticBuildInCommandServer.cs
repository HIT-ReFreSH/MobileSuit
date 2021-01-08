using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.Logging;

namespace PlasticMetal.MobileSuit.ObjectModel.Premium
{
    /// <summary>
    ///     A build-in-command server for Diagnostic
    /// </summary>
    public interface IDiagnosticBuildInCommandServer : IBuildInCommandServer
    {
        /// <summary>
        ///     The log driver to view logs
        /// </summary>
        public LogDriver Log { get; }
    }
}