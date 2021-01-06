namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Status of the last Commandline. Return value type for Built-In-Commands and Host Functions.
    /// </summary>
    public enum TraceBack
    {
        /// <summary>
        ///     Needs more input
        /// </summary>
        Prompt = 2,

        /// <summary>
        ///     The Progress is Exiting
        /// </summary>
        OnExit = 1,

        /// <summary>
        ///     Everything is OK
        /// </summary>
        AllOk = 0,

        /// <summary>
        ///     This is not a command.
        /// </summary>
        InvalidCommand = -1,

        /// <summary>
        ///     Cannot find the object referring to.
        /// </summary>
        ObjectNotFound = -2,

        /// <summary>
        ///     Cannot find the member in the object referring to.
        /// </summary>
        MemberNotFound = -3,

        /// <summary>
        ///     Error in the application
        /// </summary>
        ApplicationError = -4
    }
}