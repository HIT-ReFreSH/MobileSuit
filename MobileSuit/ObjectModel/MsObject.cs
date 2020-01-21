#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    using Members;
    public class MsObject : IExecutable, IEnumerable<(string, ObjectMember)>
    {
        public object? Instance { get; }

        private SortedList<string, List<(string, ObjectMember)>> Members { get; } = new
            SortedList<string, List<(string, ObjectMember)>>();


        private void TryAddMember(ObjectMember? objMember)
        {
            if (objMember?.Access != MemberAccess.VisibleToUser)
                return;
            if (objMember.AbsoluteName.Length > 4 && (objMember.AbsoluteName[..4] == "get_" || objMember.AbsoluteName[..4] == "set_")) return;
            foreach (var name in objMember.FriendlyNames)
            {
                var lName = name.ToLower();
                if (Members.ContainsKey(lName))
                {
                    Members[lName].Add((name,objMember));
                }
                else
                {
                    Members.Add(lName, new List<(string, ObjectMember)> {(name, objMember)});
                }
            }

        }
        public MsObject(object? instance)
        {
            Instance = instance;
            var type = instance?.GetType();
            if (type is null) return;
            foreach (var member in type.GetMembers(IExecutable.Flags))
            {
                TryAddMember(member switch
                {
                    FieldInfo field => new ContainerMember(instance, field),
                    MethodInfo method => new ExecutableMember(instance, method),
                    PropertyInfo property => new ContainerMember(instance, property),
                    _ => null
                });
            }
            
        }
        public TraceBack Execute(string[] args)
        {

            if (args.Length == 0) return TraceBack.ObjectNotFound;
            args[0] = args[0].ToLower();
            if(!Members.ContainsKey(args[0])) return TraceBack.ObjectNotFound;
            foreach (var (_,exe) in Members[args[0]])
            {
                var r = exe.Execute(args[1..]);
                if (r == TraceBack.ObjectNotFound) continue;
                return r;
            }
            return TraceBack.ObjectNotFound;
        }
        private class Enumerator: IEnumerator<(string, ObjectMember)>
        {
            private IEnumerator<KeyValuePair<string, List<(string, ObjectMember)>>> ObjectEnumerator { get; }
            private IEnumerator<(string, ObjectMember)>? CurrentEnumerator { get; set; }
            public Enumerator(MsObject obj)
            {
                ObjectEnumerator = obj.Members.GetEnumerator();
            }
            public bool MoveNext()
            {
                if (CurrentEnumerator is null || !CurrentEnumerator.MoveNext())
                {
                    if (!ObjectEnumerator.MoveNext()) return false;
                    CurrentEnumerator?.Dispose();
                    CurrentEnumerator = ObjectEnumerator.Current.Value.GetEnumerator();

                }
                CurrentEnumerator.MoveNext();
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

        public IEnumerator<(string, ObjectMember)> GetEnumerator() => new Enumerator(this);
        public int MemberCount => Members.Count;
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
