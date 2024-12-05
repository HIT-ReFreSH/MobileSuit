using System;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit;

/// <summary>
///     To configure Options of IOHub
/// </summary>
public delegate void IIOHubConfigurator(IIOHub hub);

/// <summary>
///     Featured Options of IOHub
/// </summary>
[Flags]
public enum IOOptions : ulong
{
    /// <summary>
    ///     No feature applied.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Suggests no prompt should be output to the stream
    /// </summary>
    DisablePrompt = 1 << 1,

    /// <summary>
    ///     Suggests no type/time tag should be output to the stream
    /// </summary>
    DisableTag = 1 << 2,

    /// <summary>
    ///     Suggests no Line prefix should be output to the stream
    /// </summary>
    DisableLinePrefix = 1 << 3
}

/// <summary>
///     A entity, which serves the input/output of a mobile suit.
/// </summary>
public interface IIOHub : IIOHubYouShouldNeverUse;