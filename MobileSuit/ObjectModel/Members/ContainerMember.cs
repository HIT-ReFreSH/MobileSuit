#nullable enable
using System;
using System.Reflection;
using System.Text;

namespace PlasticMetal.MobileSuit.ObjectModel.Members
{
    public class ContainerMember : ObjectMember
    {
        private Func<object?, object?> GetValue { get; }
        private MobileSuitObject? Members { get; set; }
        private MobileSuitInfoAttribute? InfoA { get; }
        public ContainerMember(object? instance, PropertyInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            InfoA = info.GetCustomAttribute<MobileSuitInfoAttribute>();
            Information = InfoA?.Prompt ?? "...";
            Type = InfoA is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo;
        }
        public ContainerMember(object? instance, FieldInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            InfoA = info.GetCustomAttribute<MobileSuitInfoAttribute>();
            Information = InfoA?.Prompt ?? "...";
            Type = InfoA is null? MemberType.FieldWithoutInfo: MemberType.FieldWithInfo;
        }


        public override TraceBack Execute(string[] args)
        {
            Members = new MobileSuitObject(GetValue(Instance));
            
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
                Information = InfoA.Prompt;

            }
            return Members.Execute(args);
        }
    }
}