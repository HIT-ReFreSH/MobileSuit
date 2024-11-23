using System.Threading.Tasks;

namespace HitRefresh.MobileSuit.Core;

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
    RequestStatus ExitSuit();

    /// <summary>
    ///     Join a Running task
    /// </summary>
    /// <param name="index"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<string?> Join(int index, SuitContext context);

    /// <summary>
    ///     Get All tasks
    /// </summary>
    /// <returns></returns>
    Task Tasks();

    /// <summary>
    ///     Stop a Running task
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Task Stop(int index);

    /// <summary>
    ///     Stop a Running task
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Task Logs(int index);

    /// <summary>
    ///     Clear all Completed Tasks.
    /// </summary>
    /// <returns></returns>
    Task ClearCompleted();

    /// <summary>
    ///     Get current directory
    /// </summary>
    /// <returns></returns>
    string Dir();

    /// <summary>
    ///     Set current directory
    /// </summary>
    /// <returns></returns>
    string ChDir(string path);

    /// <summary>
    ///     Show Help of MobileSuit
    /// </summary>
    /// <param name="args">command args</param>
    /// <returns>Command status</returns>
    Task Help(string[] args);
}