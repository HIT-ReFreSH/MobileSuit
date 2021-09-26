#nullable enable
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Parsing;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Stands for SuitObject's members which can also be SuitObjects (Field/Property).
    /// </summary>
    public class ContainerMember : Member
    {
        private SuitShell? _msValue;

        /// <summary>
        ///     Initialize an Object's Member with its instance and Property's information.
        /// </summary>
        /// <param name="instance">Instance of Object.</param>
        /// <param name="info">Object's member(Property)'s information</param>
        public ContainerMember(object? instance, PropertyInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            SetValue = info.SetValue;
            ValueType = info.PropertyType;
            Converter = info.GetCustomAttribute<SuitParserAttribute>()?.Converter;
            InfoA = info.GetCustomAttribute<SuitInfoAttribute>();
            Information = InfoA?.Text ?? "...";
            Type = InfoA is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo;
        }

        /// <summary>
        ///     Initialize an Object's Member with its instance and Field's information.
        /// </summary>
        /// <param name="instance">Instance of Object.</param>
        /// <param name="info">Object's member(Field)'s information</param>
        public ContainerMember(object? instance, FieldInfo info) : base(instance, info)
        {
            GetValue = info.GetValue;
            SetValue = info.SetValue;
            ValueType = info.FieldType;
            Converter = info.GetCustomAttribute<SuitParserAttribute>()?.Converter;
            InfoA = info.GetCustomAttribute<SuitInfoAttribute>();
            Information = InfoA?.Text ?? "...";
            Type = InfoA is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo;
        }

        /// <summary>
        ///     Member's value as a SuitObject
        /// </summary>
        public SuitShell SuitValue => _msValue ??= new SuitShell(Value);

        /// <summary>
        ///     Type of Member's value
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        ///     Member's value.
        /// </summary>
        public object? Value
        {
            get => GetValue(Instance);
            set => SetValue(Instance, value);
        }

        /// <summary>
        ///     Converter which can convert String to value's type of this member.
        /// </summary>
        public Converter<string, object>? Converter { get; }

        private Func<object?, object?> GetValue { get; }
        private Action<object?, object?> SetValue { get; }
        private SuitInfoAttribute? InfoA { get; }

        
        /// <inheritdoc/>
        public override Task<ExecuteResult> Execute(string[] args, CancellationToken token)
        {
            if (InfoA is null)
            {
                var infoSb = new StringBuilder();
                if (SuitValue.MemberCount > 0)
                {
                    var i = 0;
                    foreach (var (name, member) in SuitValue)
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

            return SuitValue
                .Execute(args, token);
        }
    }
}