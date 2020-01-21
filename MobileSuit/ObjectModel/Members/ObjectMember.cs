#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel.Members
{
    public enum MemberAccess
    {
        Hidden = 0,
        VisibleToUser = 1
    }

    public enum MemberType
    {
        MethodWithInfo=0,
        MethodWithoutInfo=-1,
        FieldWithInfo=1,
        FieldWithoutInfo=-2
    }
    public abstract class ObjectMember:IExecutable
    {
        public MemberAccess Access { get; }
        public MemberType Type { get; protected set; }
            = MemberType.MethodWithInfo;
        public string Information { get; protected set; }
            = "";
        public string[] FriendlyNames { get; protected set; }
        public string AbsoluteName { get; protected set; }
        public object? Instance { get; set; }
        public abstract TraceBack Execute(string[] args);
        protected ObjectMember(object? instance, MemberInfo member)
        {
            
            Access = member.GetCustomAttribute<MsIgnorableAttribute>() is null
                ? MemberAccess.VisibleToUser
                : MemberAccess.Hidden;
            AbsoluteName = member.Name;
            FriendlyNames = (
                from a in member.GetCustomAttributes<MsAliasAttribute>(true)
                select a.Text).Union(new[]{
                AbsoluteName
            }).ToArray();
            Instance = instance;
        }
    }
}
