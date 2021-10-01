namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Status of the last Commandline. Return value type for Built-In-Commands and Host Functions.
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        ///     No Request is input by the user.
        /// </summary>
        NoRequest = 1,

        /// <summary>
        ///     The Progress is Exiting
        /// </summary>
        OnExit = -1,

        /// <summary>
        ///     Everything is OK
        /// </summary>
        Ok = 0,

        /// <summary>
        ///     Everything is OK
        /// </summary>
        NotHandled = 2,

        /// <summary>
        ///     Command is Running. Set by the FinalMiddleware.
        /// </summary>
        Running = 3,

        /// <summary>
        ///     Command is Running. Set by the FinalMiddleware.
        /// </summary>
        Handled = 4,

        /// <summary>
        ///     Cannot find the object referring to.
        /// </summary>
        Interrupt = -2,

        /// <summary>
        ///     Cannot find the member in the object referring to.
        /// </summary>
        CommandNotFound = -3,

        /// <summary>
        ///     Error in the application
        /// </summary>
        Faulted = -4,

        /// <summary>
        ///     Failed to parse an argument of a command.
        /// </summary>
        CommandParsingFailure = -5
    }
}