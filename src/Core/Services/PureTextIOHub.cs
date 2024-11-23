// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System.Threading.Tasks;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     IO hub with pure text output.
/// </summary>
public class PureTextIOHub : TrueColorIOHub
{
    /// <summary>
    ///     Initialize a IOhub.
    /// </summary>
    public PureTextIOHub(PromptFormatter promptFormatter, IIOHubConfigurator configurator) : base
    (
        promptFormatter,
        configurator
    ) { }

    /// <inheritdoc />
    public override void Write(PrintUnit content) { Output.Write(content.Text); }

    /// <inheritdoc />
    public override async Task WriteAsync(PrintUnit content) { await Output.WriteAsync(content.Text); }
}