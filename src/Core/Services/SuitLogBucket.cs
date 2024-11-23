// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System.Collections.Generic;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     A bucket to contain logs
/// </summary>
public interface ISuitLogBucket : IList<PrintUnit>;

internal class SuitLogBucket : List<PrintUnit>, ISuitLogBucket;