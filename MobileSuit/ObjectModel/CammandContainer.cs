#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    using Members;
    public class MobileSuitObject : IExecutable, IEnumerable<(string, ObjectMember)>
    {
        public object? Instance { get; }
        private List<(string, ObjectMember)> Members { get; } = new List<(string, ObjectMember)>();


        private void TryAddMember(ObjectMember? objMember)
        {
            if (objMember?.Access != MemberAccess.VisibleToUser) return;
            if (objMember.AbsoluteName.Length >4 && (objMember.AbsoluteName[..4] == "get_" || objMember.AbsoluteName[..4] == "set_"))return;
            foreach (var name in objMember.FriendlyNames)
            {
                Members.Add((name, objMember));
            }

        }
        public MobileSuitObject(object? instance)
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
            Members.Sort((s1, s2) => string.CompareOrdinal(s1.Item1.ToLower(), s2.Item1.ToLower()));
        }
        public TraceBack Execute(string[] args)
        {
            
            if (args.Length == 0) return TraceBack.ObjectNotFound;
            args[0] = args[0].ToLower();
            var left = 0;
            var right = Members.Count - 1;
            var mid = (left + right) / 2;
            var flag = false;
            while (left <= right)
            {
                var c = string.CompareOrdinal(args[0], Members[mid].Item1.ToLower());
                if (c == 0)
                {
                    flag = true;
                    break;
                }
                if (c > 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }

                mid = (left + right) >> 1;
            }//二分查找

            return flag?Members[mid].Item2.Execute(args[1..]):TraceBack.ObjectNotFound;
        }

        public IEnumerator<(string,ObjectMember)> GetEnumerator() => Members.GetEnumerator();
        public int MemberCount => Members.Count;
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
