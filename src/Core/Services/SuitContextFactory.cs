// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System;
using Microsoft.Extensions.DependencyInjection;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
/// A factory to create suit context
/// </summary>
public interface ISuitContextFactory
{
    /// <summary>
    /// Create a suit context
    /// </summary>
    /// <returns></returns>
    public SuitContext CreateContext();
}

/// <inheritdoc/>
public class SuitContextFactory(IServiceProvider serviceProvider) : ISuitContextFactory
{
    /// <inheritdoc/>
    public SuitContext CreateContext() { return new SuitContext(serviceProvider.CreateScope()); }
}