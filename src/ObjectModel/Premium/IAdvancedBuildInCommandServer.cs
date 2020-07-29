using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel.Premium
{
    /// <summary>
    /// A BuildInCommandServer providing shell and host interfaces
    /// </summary>
    public interface IAdvancedBuildInCommandServer:IBuildInCommandServer
    {



        /// <summary>
        ///     Switch Options for MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack SwitchOption(string[] args);

 

        /// <summary>
        ///     Output something in default way
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Print(string[] args);

        /// <summary>
        ///     A more powerful way to output something, with arg1 as option
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack SuperPrint(string[] args);

        /// <summary>
        ///     Execute command with the System Shell
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        TraceBack Shell(string[] args);

    }
}
