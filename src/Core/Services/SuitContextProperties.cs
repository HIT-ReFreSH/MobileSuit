// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System.Collections.Generic;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     Provides a way to access <see cref="SuitContext" />.<see cref="SuitContext.Properties" />.
///     Well, who is who?
/// </summary>
public interface ISuitContextProperties : IDictionary<string, string>;

internal class SuitContextProperties : Dictionary<string, string>, ISuitContextProperties;