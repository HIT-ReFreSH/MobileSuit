using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HitRefresh.MobileSuit.Core;

/// <summary>
///     A HostApplicationLifetime for MobileSuit
/// </summary>
public class SuitHostApplicationLifetime : IHostApplicationLifetime
{
    private readonly Func<Task> _stopCallback;

    /// <summary>
    ///     Describe a lifetime of MobileSuit
    /// </summary>
    /// <param name="startTask"></param>
    /// <param name="stopCallback"></param>
    public SuitHostApplicationLifetime(TaskCompletionSource startTask, Func<Task> stopCallback)
    {
        _stopCallback = stopCallback;
        ApplicationStartedSource = new CancellationTokenSource();
        startTask.Task.ContinueWith(_ => ApplicationStartedSource.Cancel());
        ApplicationStoppingSource = new CancellationTokenSource();
        ApplicationStoppedSource = new CancellationTokenSource();
    }

    private CancellationTokenSource ApplicationStartedSource { get; }
    private CancellationTokenSource ApplicationStoppingSource { get; }
    private CancellationTokenSource ApplicationStoppedSource { get; }

    /// <inheritdoc />
    public async void StopApplication()
    {
        if (ApplicationStoppingSource.IsCancellationRequested) return;
        ApplicationStoppingSource.Cancel();
        await _stopCallback();
        ApplicationStoppedSource.Cancel();
    }

    /// <inheritdoc />
    public CancellationToken ApplicationStarted => ApplicationStartedSource.Token;

    /// <inheritdoc />
    public CancellationToken ApplicationStopping => ApplicationStoppingSource.Token;

    /// <inheritdoc />
    public CancellationToken ApplicationStopped => ApplicationStoppedSource.Token;
}