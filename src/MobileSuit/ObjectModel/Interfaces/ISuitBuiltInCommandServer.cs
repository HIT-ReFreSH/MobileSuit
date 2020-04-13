namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    /// <summary>
    /// Build-In-Command's model
    /// </summary>
    /// <param name="args">Arguments for the command</param>
    /// <returns>Command's TraceBack result.</returns>
    public delegate TraceBack BuildInCommand(string[] args);
    /// <summary>
    /// Built-In-Command Server's Model.
    /// </summary>
    public interface ISuitBuiltInCommandServer
    {
        /// <summary>
        /// Enter a member of Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Enter(string[] args);

        /// <summary>
        /// Leave the Current SuitObject, Back to its Parent
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Leave(string[] args);

        /// <summary>
        /// Create and Enter a new SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack New(string[] args);

        /// <summary>
        /// Show Certain Member's Value of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack View(string[] args);

        /// <summary>
        /// Run SuitScript at the given location
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack RunScript(string[] args);

        /// <summary>
        /// Switch Options for MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack SwitchOption(string[] args);

        /// <summary>
        /// Modify Certain Member's Value of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack ModifyMember(string[] args);

        /// <summary>
        /// Show Members of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack List(string[] args);

        /// <summary>
        /// Free the Current SuitObject, and back to the last one.
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Free(string[] args);

        /// <summary>
        /// Exit MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Exit(string[] args);

        /// <summary>
        /// Show Current SuitObject Information
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack This(string[] args);

        /// <summary>
        /// Output something in default way
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Print(string[] args);

        /// <summary>
        /// A more powerful way to output something, with arg1 as option
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack SuperPrint(string[] args);

        /// <summary>
        /// Execute command with the System Shell
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Shell(string[] args);

        /// <summary>
        /// Show Help of MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Help(string[] args);
    }
}