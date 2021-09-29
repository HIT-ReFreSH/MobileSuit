using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel.Premium
{
    /// <summary>
    ///     A Build-in-command server for dynamic host, containing free and create-object
    /// </summary>
    public interface IDynamicBuildInCommandServer : ISuitCommandServer
    {
        /// <summary>
        ///     Create and Enter a new SuitObject
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus CreateObject(string[] args);


        /// <summary>
        ///     Free the Current SuitObject, and back to the last one.
        /// </summary>
        /// <param name="args">command args</param>
        /// <returns>Command status</returns>
        RequestStatus Free(string[] args);
    }
}