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
            host.Logger.EnableLogQuery = true;
        }

        /// <inheritdoc />
        [SuitInfo(typeof(LogRes), "Server")]
        public LogDriver Log => new LogDriver(Host.Logger);
    }
}