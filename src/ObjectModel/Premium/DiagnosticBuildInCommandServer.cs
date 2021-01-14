using PlasticMetal.MobileSuit.Logging;

namespace PlasticMetal.MobileSuit.ObjectModel.Premium
{
    /// <summary>
    ///     Build in command server for diagnostic
    /// </summary>
    public class DiagnosticBuildInCommandServer : BuildInCommandServer, IDiagnosticBuildInCommandServer
    {
        /// <inheritdoc />
        public DiagnosticBuildInCommandServer(SuitHost host) : base(host)
        {
        }

        /// <inheritdoc />
        [SuitInfo(typeof(LogRes), "Server")]
        public LogDriver Log 
            => new(Host.Logger as CachedLogger?? 
                   throw new System.Exception(Lang.LogNotCached));
    }
}