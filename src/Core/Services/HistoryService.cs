namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     Provides request history.
/// </summary>
public interface IHistoryService
{
    /// <summary>
    ///     Status of last Request.
    /// </summary>
    public RequestStatus Status { get; set; }

    /// <summary>
    ///     Response of last Request.
    /// </summary>
    public string? Response { get; set; }
}

internal class HistoryService : IHistoryService
{
    public RequestStatus Status { get; set; } = RequestStatus.Ok;
    public string? Response { get; set; }
}