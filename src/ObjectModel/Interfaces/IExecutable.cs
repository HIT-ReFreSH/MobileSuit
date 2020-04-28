namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    /// <summary>
    /// Represents an entity which can be executed.
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        /// Execute this object.
        /// </summary>
        /// <param name="args">The arguments for execution.</param>
        /// <param name="returnValue">the return value of execute.</param>
        /// <returns>TraceBack result of this object.</returns>
        public TraceBack Execute(string[] args, out object? returnValue);
    }
}