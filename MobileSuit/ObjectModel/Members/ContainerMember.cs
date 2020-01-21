#nullable enable
using System;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel.Members
{
    public class ContainerMember : ObjectMember
    {
        public MsObject MsValue => _msValue ??= new MsObject(Value);
        public Type ValueType { get; }
        private MsObject? _msValue;
        public object? Value
        {
            get => GetValue(Instance);
            set => SetValue(Instance, value);
        }
        public Converter<string, object>? Converter { get; }
        private Func<object?, object?> GetValue { get; }
        private Action<object?, object?> SetValue { get; }
        private MsInfoAttribute? InfoA { get; }
        public ContainerMember(object? instance, PropertyInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            SetValue = info.SetValue;
            ValueType = info.PropertyType;
            Converter = info.GetCustomAttribute<MsParserAttribute>()?.Converter;
            InfoA = info.GetCustomAttribute<MsInfoAttribute>();
            Information = InfoA?.Text ?? "...";
            Type = InfoA is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo;
        }
        public ContainerMember(object? instance, FieldInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            SetValue = info.SetValue;
            ValueType = info.FieldType;
            Converter = info.GetCustomAttribute<MsParserAttribute>()?.Converter;
            InfoA = info.GetCustomAttribute<MsInfoAttribute>();
            Information = InfoA?.Text ?? "...";
            Type = InfoA is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo;
        }


        public override TraceBack Execute(string[] args)
        {

            if (InfoA is null)
            {

                var infoSb = new StringBuilder();
                if (MsValue.MemberCount > 0)
                {
                    var i = 0;
                    foreach (var (name, member) in MsValue)
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
            return MsValue
                .Execute(args);
        }
    }
}