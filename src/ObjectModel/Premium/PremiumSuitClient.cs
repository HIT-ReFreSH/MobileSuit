namespace PlasticMetal.MobileSuit.ObjectModel.Premium
{
    /// <summary>
    ///     An Premium mobile suit client driver-class, has full access to host
    /// </summary>
    public abstract class HostInteractiveClient : IHostInteractive
    {
        /// <inheritdoc />
        [SuitIgnore]
        public IAssignOnceHost Host { get; } = new AssignOnceHost();
    }
}