#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;
using PlasticMetal.MobileSuit.ObjectModel.Members;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// Represents an object in Mobile Suit.
    /// </summary>
    public class MsObject : IExecutable, IEnumerable<(string, ObjectMember)>
    {
        private const BindingFlags Flags = BindingFlags.IgnoreCase
                                           | BindingFlags.DeclaredOnly
                                           | BindingFlags.Public
                                           | BindingFlags.Instance
                                           | BindingFlags.Static;

        /// <summary>
        /// Initialize a MsObject with an instance.
        /// </summary>
        /// <param name="instance">The instance that this MsObject represents.</param>
        public MsObject(object? instance)
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
        /// The instance that this MsObject represents.
        /// </summary>
        public object? Instance { get; }

        private SortedList<string, List<(string, ObjectMember)>> Members { get; } = new
            SortedList<string, List<(string, ObjectMember)>>();

        private SortedList<string, List<(string, ObjectMember)>> MembersAbs { get; } = new
            SortedList<string, List<(string, ObjectMember)>>();
        /// <summary>
        /// Count of Members that this object contains.
        /// </summary>
        public int MemberCount => Members.Count;

        /// <summary>
        /// Get Enumerator of its (AbsoluteName,Member)[]
        /// </summary>
        /// <returns>Enumerator of its (AbsoluteName,Member)[]</returns>
        public IEnumerator<(string, ObjectMember)> GetEnumerator()
        {
            return new Enumerator(this);
        }
        /// <summary>
        /// Get Enumerator of its (AbsoluteName,Member)[]
        /// </summary>
        /// <returns>Enumerator of its (AbsoluteName,Member)[]</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Execute this object.
        /// </summary>
        /// <param name="args">The arguments for execution.</param>
        /// <returns>TraceBack result of this object.</returns>
        public TraceBack Execute(string[] args)
        {
            if (args.Length == 0) return TraceBack.ObjectNotFound;
            args[0] = args[0].ToLower();
            if (!Members.ContainsKey(args[0])) return TraceBack.ObjectNotFound;
            foreach (var (_, exe) in Members[args[0]])
            {
                var r = exe.Execute(args[1..]);
                if (r == TraceBack.ObjectNotFound) continue;
                return r;
            }

            return TraceBack.ObjectNotFound;
        }

        private void TryAddMember(ObjectMember? objMember)
        {
            if (objMember?.Access != MemberAccess.VisibleToUser)
                return;
            if (objMember.AbsoluteName.Length > 4
                && (objMember.AbsoluteName[..4] == "get_" || objMember.AbsoluteName[..4] == "set_")) return;
            var lAbsName = objMember.AbsoluteName.ToLower();
            if (MembersAbs.ContainsKey(lAbsName)) MembersAbs[lAbsName].Add((objMember.AbsoluteName, objMember));
            else MembersAbs.Add(lAbsName, new List<(string, ObjectMember)> {(objMember.AbsoluteName, objMember)});
            foreach (var name in objMember.FriendlyNames)
            {
                var lName = name.ToLower();
                if (Members.ContainsKey(lName)) Members[lName].Add((name, objMember));
                else Members.Add(lName, new List<(string, ObjectMember)> {(name, objMember)});
            }
        }
        /// <summary>
        /// Try to get the field/property with certain name.
        /// </summary>
        /// <param name="name">Name the field/property.</param>
        /// <param name="field">The field/property with the certain name.</param>
        /// <returns>TraceBack of the find operation.</returns>
        public TraceBack TryGetField(string name, out ContainerMember? field)
        {
            if (!Members.ContainsKey(name.ToLower()))
            {
                field = null;
                return TraceBack.ObjectNotFound;
            }

            field = Members[name][0].Item2 as ContainerMember;
            return field is null ? TraceBack.ObjectNotFound : TraceBack.AllOk;
        }

        private class Enumerator : IEnumerator<(string, ObjectMember)>
        {
            public Enumerator(MsObject obj)
            {
                ObjectEnumerator = obj.MembersAbs.GetEnumerator();
            }

            private IEnumerator<KeyValuePair<string, List<(string, ObjectMember)>>> ObjectEnumerator { get; }
            private IEnumerator<(string, ObjectMember)>? CurrentEnumerator { get; set; }

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

            public (string, ObjectMember) Current { get; private set; }

            object? IEnumerator.Current => Current;

            public void Dispose()
            {
                ObjectEnumerator.Dispose();
                CurrentEnumerator?.Dispose();
            }
        }
    }
}