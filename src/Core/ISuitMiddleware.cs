using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core;

/// <summary>
///     A function to handle a suit request.
/// </summary>
/// <param name="context">Context of the request.</param>
/// <returns></returns>
public delegate Task SuitRequestDelegate(SuitContext context);

/// <summary>
///     A middleware of Mobile Suit.
/// </summary>
public interface ISuitMiddleware
{
    /// <summary>
    ///     To invoke the middleware
    /// </summary>
    /// <param name="context">Context of the request.</param>
    /// <param name="next">next Middleware</param>
    /// <returns></returns>
    public Task InvokeAsync(SuitContext context, SuitRequestDelegate next);
}