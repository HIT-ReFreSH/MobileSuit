#nullable enable
using System;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel.Members
{
    public class ContainerMember : ObjectMember
    {
        private Func<object?, object?> GetValue { get; }
        private MsObject? Members { get; set; }
        private MsInfoAttribute? InfoA { get; }
        public ContainerMember(object? instance, PropertyInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            InfoA = info.GetCustomAttribute<MsInfoAttribute>();
            Information = InfoA?.Text ?? "...";
            Type = InfoA is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo;
        }
        public ContainerMember(object? instance, FieldInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            InfoA = info.GetCustomAttribute<MsInfoAttribute>();
            Information = InfoA?.Text ?? "...";
            Type = InfoA is null? MemberType.FieldWithoutInfo: MemberType.FieldWithInfo;
        }


        public override TraceBack Execute(string[] args)
        {
            Members = new MsObject(GetValue(Instance));
            
            if (InfoA is null)
            {

                var infoSb = new StringBuilder();
                if (Members.MemberCount > 0)
                {
                    var i = 0;
                    foreach (var (name, member) in Members)
                    {
                        infoSb.Append(name);
                        infoSb.Append(member switch
                        {
                            ExecutableMember e => "()",
                            ContainerMember c => "{}",
                            _ => ""
                        });
                        infoSb.Append(',');
                        if (++i > 5)
                        {
                            infoSb.Append("...,");
                            break;
                        }
                    }

                    Information = infoSb.ToString()[..^1];

                }
            }
            else
            {
                Information = InfoA.Text;

            }
            return Members.Execute(args);
        }
    }
}