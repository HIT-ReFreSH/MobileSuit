using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitRefresh.MobileSuit.Core.Services;

/// <summary>
///     SuitShell for Client App.
/// </summary>
public class SuitAppShell : SuitShell, ISuitShellCollection
{
    private readonly List<SuitShell> _members = new();

    private SuitAppShell() : base(typeof(object), _ => null, "SuitClient") { }

    /// <inheritdoc />
    public override int MemberCount => _members.Count;

    /// <summary>
    ///     Ordered members of this
    /// </summary>
    public IEnumerable<SuitShell> Members()
    {
        foreach (var shell in _members)
            switch (shell)
            {
            case SuitMethodShell method:
                yield return method;
                break;
            case SuitObjectShell obj:
            {
                foreach (var objMember in obj.Members()) yield return objMember;

                break;
            }
            default:
                yield return shell;
                break;
            }
    }

    internal static SuitAppShell FromClients(IEnumerable<SuitShell> clients)
    {
        var r = new SuitAppShell();
        r._members.AddRange(clients);
        return r;
    }

    /// <inheritdoc />
    public override async Task Execute(SuitContext context)
    {
        foreach (var shell in _members)
        {
            await shell.Execute(context);
            if (context.Status != RequestStatus.NotHandled) return;
        }
    }

    /// <inheritdoc />
    public override bool MayExecute(IReadOnlyList<string> request)
    {
        return _members.Any(sys => sys.MayExecute(request));
    }
}