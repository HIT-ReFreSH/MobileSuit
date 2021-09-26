namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Build-In-Command's model
    /// </summary>
    /// <param name="args">Arguments for the command</param>
    /// <returns>Command's TraceBack result.</returns>
    public delegate RequestStatus BuildInCommand(string[] args);

    /// <summary>
    ///     Built-In-Command Server's Model.
    /// </summary>
    public interface IBuildInCommandServer
    {
        /// <summary>
        ///     Enter a member of Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus Enter(string[] args);

        /// <summary>
        ///     Leave the Current SuitObject, Back to its Parent
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus Leave(string[] args);


        /// <summary>
        ///     Show Certain Member's Value of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus View(string[] args);

        /// <summary>
        ///     Run SuitScript at the given location
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus RunScript(string[] args);


        /// <summary>
        ///     Modify Certain Member's Value of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus ModifyMember(string[] args);

        /// <summary>
        ///     Show Members of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus List(string[] args);


        /// <summary>
        ///     Exit MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus ExitSuit(string[] args);

        /// <summary>
        ///     Show Current SuitObject Information
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus This(string[] args);


        /// <summary>
        ///     Show Help of MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus Help(string[] args);
    }
}