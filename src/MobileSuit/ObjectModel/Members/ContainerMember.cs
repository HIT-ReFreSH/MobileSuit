#nullable enable
using System;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel.Members
{
    /// <summary>
    /// Stands for MsObject's members which can also be MsObjects (Field/Property).
    /// </summary>
    public class ContainerMember : ObjectMember
    {
        private MsObject? _msValue;
        /// <summary>
        /// Initialize an Object's Member with its instance and Property's information.
        /// </summary>
        /// <param name="instance">Instance of Object.</param>
        /// <param name="info">Object's member(Property)'s information</param>
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
        /// <summary>
        /// Initialize an Object's Member with its instance and Field's information.
        /// </summary>
        /// <param name="instance">Instance of Object.</param>
        /// <param name="info">Object's member(Field)'s information</param>
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
        /// <summary>
        /// Member's value as a MsObject
        /// </summary>
        public MsObject MsValue => _msValue ??= new MsObject(Value);
        /// <summary>
        /// Type of Member's value
        /// </summary>
        public Type ValueType { get; }
        /// <summary>
        /// Member's value.
        /// </summary>
        public object? Value
        {
            get => GetValue(Instance);
            set => SetValue(Instance, value);
        }
        /// <summary>
        /// Converter which can convert String to value's type of this member.
        /// </summary>
        public Converter<string, object>? Converter { get; }
        private Func<object?, object?> GetValue { get; }
        private Action<object?, object?> SetValue { get; }
        private MsInfoAttribute? InfoA { get; }

        /// <summary>
        /// Execute this object.
        /// </summary>
        /// <param name="args">The arguments for execution.</param>
        /// <returns>TraceBack result of this object.</returns>
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