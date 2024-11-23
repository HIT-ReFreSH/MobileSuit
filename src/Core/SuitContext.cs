using System;
using System.Threading;
using HitRefresh.MobileSuit.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HitRefresh.MobileSuit.Core;

/// <summary>
///     Context through the lifetime of a SuitRequest(A Command)
/// </summary>
public class SuitContext : IDisposable
{
    private readonly IServiceScope _serviceScope;

    /// <summary>
    ///     Create SuitContext with specified Service Scope
    /// </summary>
    /// <param name="scope"></param>
    public SuitContext(IServiceScope scope)
    {
        _serviceScope = scope;
        ServiceProvider = scope.ServiceProvider;
        CancellationToken = new CancellationTokenSource();
        var hostLifeTime = ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
        Properties = ServiceProvider.GetRequiredService<ISuitContextProperties>();
        hostLifeTime.ApplicationStopping.Register(() => CancellationToken.Cancel());
    }

    /// <summary>
    ///     CancellationToken of the request.
    /// </summary>
    public CancellationTokenSource CancellationToken { get; }

    /// <summary>
    ///     Properties of current request.
    /// </summary>
    public ISuitContextProperties Properties { get; }

    /// <summary>
    ///     The execution status of current request.
    /// </summary>
    public RequestStatus Status { get; set; } = RequestStatus.NoRequest;

    /// <summary>
    ///     The exception caught in the execution.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    ///     A command from input stream
    /// </summary>
    public string[] Request { get; set; } = Array.Empty<string>();

    /// <summary>
    ///     The ServiceProvider who provides services through whole request.
    /// </summary>
    public IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    ///     Output to the output stream
    /// </summary>
    public string? Response { get; set; }

    /// <inheritdoc />
    public void Dispose() { _serviceScope.Dispose(); }
}