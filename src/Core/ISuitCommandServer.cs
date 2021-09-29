using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Built-In-Command Server's Model.
    /// </summary>
    public interface ISuitCommandServer
    {


        /// <summary>
        ///     Show Members of the Current SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        Task ListCommands(string[] args);


        /// <summary>
        ///     Exit MobileSuit
        /// </summary>
        /// <returns>Command status</returns>
        Task ExitSuit();


        /// <summary>
        ///     Show Help of MobileSuit
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        Task Help(string[] args);
    }
}