using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core;

/// <summary>
/// A HostApplicationLifetime for MobileSuit
/// </summary>
internal class SuitHostApplicationLifetime: IHostApplicationLifetime
{
    private readonly Func<Task> _stopCallback;

    internal SuitHostApplicationLifetime(TaskCompletionSource startTask, Func<Task> stopCallback)
    {
        _stopCallback = stopCallback;
        ApplicationStartedSource = new();
        startTask.Task.ContinueWith(_ => ApplicationStartedSource.Cancel());
        ApplicationStoppingSource = new();
        ApplicationStoppedSource = new();

    }
    /// <inheritdoc/>
    public async void StopApplication()
    {
        if (ApplicationStartedSource.IsCancellationRequested) return;
        ApplicationStoppingSource.Cancel();
        await _stopCallback();
        ApplicationStoppedSource.Cancel();
    }

    private CancellationTokenSource ApplicationStartedSource{ get; }
    private CancellationTokenSource ApplicationStoppingSource { get; }
    private CancellationTokenSource ApplicationStoppedSource { get; }
    /// <inheritdoc/>
    public CancellationToken ApplicationStarted =>ApplicationStartedSource.Token;
    /// <inheritdoc/>
    public CancellationToken ApplicationStopping =>ApplicationStoppingSource.Token;
    /// <inheritdoc/>
    public CancellationToken ApplicationStopped =>ApplicationStoppedSource.Token;
}