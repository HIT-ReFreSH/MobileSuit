using System.Collections.Generic;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit.UI;

/// <summary>
///     Represents a Parameter which can be parsed from a String[].
/// </summary>
public interface IDynamicParameter
{
    /// <summary>
    ///     Parse this Parameter from String[].
    /// </summary>
    /// <param name="args"></param>
    /// <param name="context"></param>
    /// <returns>Whether the parsing is successful</returns>
    bool Parse(IReadOnlyList<string> args, SuitContext context);
}