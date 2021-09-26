#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Represents an object in Mobile Suit.
    /// </summary>
    public class SuitShell : IExecutable, IEnumerable<(string, Member)>
    {
        /// <summary>
        ///     The BindingFlags stands for IgnoreCase, DeclaredOnly, Public and Instance members
        /// </summary>
        public const BindingFlags Flags = BindingFlags.IgnoreCase
                                          //| BindingFlags.DeclaredOnly
                                          | BindingFlags.Public
                                          | BindingFlags.Instance

            //| BindingFlags.Static
            ;

        private static readonly SortedSet<string> BlockList = new SortedSet<string>
            {"ToString", "Equals", "GetHashCode", "GetType"};

        /// <summary>
        ///     Initialize a SuitObject with an instance.
        /// </summary>
        /// <param name="instance">The instance that this SuitObject represents.</param>
        public SuitShell(object? instance)
        {
            Instance = instance;
            var type = instance?.GetType();
            if (type is null) return;
            foreach (var member in type.GetMembers(Flags))
                TryAddMember(member switch
                {
                    FieldInfo field => new ContainerMember(instance, field),
                    MethodInfo method => new ExecutableMember(instance, method),
                    PropertyInfo property => new ContainerMember(instance, property),
                    _ => null
                });
        }

        /// <summary>
        ///     The instance that this SuitObject represents.
        /// </summary>
        public object? Instance { get; }

        private SortedList<string, List<(string, Member)>> Members { get; } = new
            SortedList<string, List<(string, Member)>>();

        private SortedList<string, List<(string, Member)>> MembersAbs { get; } = new
            SortedList<string, List<(string, Member)>>();

        /// <summary>
        ///     Count of Members that this object contains.
        /// </summary>
        public int MemberCount => Members.Count;

        /// <summary>
        ///     Get Enumerator of its (AbsoluteName,Member)[]
        /// </summary>
        /// <returns>Enumerator of its (AbsoluteName,Member)[]</returns>
        public IEnumerator<(string, Member)> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        ///     Get Enumerator of its (AbsoluteName,Member)[]
        /// </summary>
        /// <returns>Enumerator of its (AbsoluteName,Member)[]</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private void TryAddMember(Member? objMember)
        {
            if (BlockList.Contains(objMember?.AbsoluteName ?? "ToString")) return;
            if (objMember?.Access != MemberAccess.VisibleToUser)
                return;
            if (objMember.AbsoluteName.Length > 4
                && (objMember.AbsoluteName[..4] == "get_" || objMember.AbsoluteName[..4] == "set_")) return;
            var lAbsName = objMember.AbsoluteName.ToLower(CultureInfo.CurrentCulture);
            if (MembersAbs.ContainsKey(lAbsName)) MembersAbs[lAbsName].Add((objMember.AbsoluteName, objMember));
            else MembersAbs.Add(lAbsName, new List<(string, Member)> {(objMember.AbsoluteName, objMember)});
            foreach (var name in objMember.FriendlyNames)
            {
                var lName = name.ToLower(CultureInfo.CurrentCulture);
                if (Members.ContainsKey(lName)) Members[lName].Add((name, objMember));
                else Members.Add(lName, new List<(string, Member)> {(name, objMember)});
            }
        }

        /// <summary>
        ///     Try to get the field/property with certain name.
        /// </summary>
        /// <param name="name">Name the field/property.</param>
        /// <param name="field">The field/property with the certain name.</param>
        /// <returns>TraceBack of the find operation.</returns>
        public RequestStatus TryGetField(string name, out ContainerMember? field)
        {
            if (name == null)
            {
                field = null;
                return RequestStatus.InvalidCommand;
            }

            if (!Members.ContainsKey(name.ToLower(CultureInfo.CurrentCulture)))
            {
                field = null;
                return RequestStatus.ObjectNotFound;
            }

            field = Members[name][0].Item2 as ContainerMember;
            return field is null ? RequestStatus.ObjectNotFound : RequestStatus.AllOk;
        }
        /// <inheritdoc/>
        public async Task<ExecuteResult> Execute(string[] args, CancellationToken token)
        {
            if (args == null || args.Length == 0)
            {
                return new ExecuteResult {TraceBack = RequestStatus.ObjectNotFound};
            }

            args[0] = args[0].ToLower(CultureInfo.CurrentCulture);
            if (!Members.ContainsKey(args[0]))
            {
                return new ExecuteResult { TraceBack = RequestStatus.ObjectNotFound };
            }

            foreach (var (_, exe) in Members[args[0]])
            {
                var r = await exe.Execute(args[1..], token);
                if (r.TraceBack == RequestStatus.ObjectNotFound) continue;
                return r;
            }

            return new ExecuteResult { TraceBack = RequestStatus.ObjectNotFound };
        }

        private class Enumerator : IEnumerator<(string, Member)>
        {
            public Enumerator(SuitShell obj)
            {
                ObjectEnumerator = obj.MembersAbs.GetEnumerator();
            }

            private IEnumerator<KeyValuePair<string, List<(string, Member)>>> ObjectEnumerator { get; }
            private IEnumerator<(string, Member)>? CurrentEnumerator { get; set; }

            public bool MoveNext()
            {
                if (CurrentEnumerator is null || !CurrentEnumerator.MoveNext())
                {
                    if (!ObjectEnumerator.MoveNext()) return false;
                    CurrentEnumerator?.Dispose();
                    CurrentEnumerator = ObjectEnumerator.Current.Value.GetEnumerator();
                    CurrentEnumerator.MoveNext();
                }


                Current = CurrentEnumerator.Current;
                return true;
            }

            public void Reset()
            {
                ObjectEnumerator.Reset();
                CurrentEnumerator?.Dispose();
                CurrentEnumerator = null;
            }

            public (string, Member) Current { get; private set; }

            object? IEnumerator.Current => Current;

            public void Dispose()
            {
                ObjectEnumerator.Dispose();
                CurrentEnumerator?.Dispose();
            }
        }
    }
}