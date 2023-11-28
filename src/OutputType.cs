namespace HitRefresh.MobileSuit;

/// <summary>
///     Type of content that writes to the output stream.
/// </summary>
public enum OutputType
{
    /// <summary>
    ///     Normal content.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     Prompt content.
    /// </summary>
    Prompt = 1,

    /// <summary>
    ///     Error content.
    /// </summary>
    Error = 2,

    /// <summary>
    ///     All-Ok content.
    /// </summary>
    Ok = 3,

    /// <summary>
    ///     Title of a list.
    /// </summary>
    Title = 4,

    /// <summary>
    ///     Normal information.
    /// </summary>
    Info = 5,

    /// <summary>
    ///     Information provided by MobileSuit.
    /// </summary>
    System = 6,

    /// <summary>
    ///     Error content.
    /// </summary>
    Warning = 7
}