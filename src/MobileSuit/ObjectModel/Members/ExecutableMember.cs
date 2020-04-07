#nullable enable
using System;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel.Members
{
    /// <summary>
    /// Represents type of the last parameter of a method
    /// </summary>
    public enum TailParameterType
    {
        /// <summary>
        /// Last parameter exists, and is quite normal.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Last parameter is an array.
        /// </summary>
        Array = 1,
        /// <summary>
        /// Last parameter implements IDynamicParameter.
        /// </summary>
        DynamicParameter = 2,
        /// <summary>
        /// Last parameter does not exist.
        /// </summary>
        NoParameter = -1
    }
    /// <summary>
    /// Object's Member which may be a method.
    /// </summary>
    public class ExecutableMember : ObjectMember
    {
        /// <summary>
        /// Initialize an Object's Member with its instance and Method's information.
        /// </summary>
        /// <param name="instance">Instance of Object.</param>
        /// <param name="method">Object's member(Method)'s information</param>
        public ExecutableMember(object? instance, MethodBase method) : base(instance, method)
        {
            Invoke = method.Invoke;
            Parameters = method.GetParameters();

            if (Parameters.Length == 0)
            {
                TailParameterType = TailParameterType.NoParameter;
            }
            else
            {
                if (Parameters[^1].ParameterType.IsArray)
                    TailParameterType = TailParameterType.Array;
                else if (!(Parameters[^1].ParameterType.GetInterface("IDynamicParameter") is null))
                    TailParameterType = TailParameterType.DynamicParameter;
                else
                    TailParameterType = TailParameterType.Normal;


                MaxParameterCount =
                    TailParameterType == TailParameterType.Normal
                        ? Parameters.Length
                        : int.MaxValue;
                NonArrayParameterCount =
                    TailParameterType == TailParameterType.Normal
                        ? Parameters.Length
                        : Parameters.Length - 1;
                var i = NonArrayParameterCount - 1;
                for (; i >= 0 && Parameters[i].IsOptional; i--)
                {
                }

                MinParameterCount = i + 1;
            }

            var info = method.GetCustomAttribute<MsInfoAttribute>();
            if (info is null)
            {
                Type = MemberType.MethodWithoutInfo;
                var infoSb = new StringBuilder();
                if (MaxParameterCount > 0)
                {
                    foreach (var parameter in Parameters)
                    {
                        infoSb.Append(parameter.Name);
                        infoSb.Append(parameter switch
                        {
                            { } when parameter.ParameterType.IsArray => "[]",
                            { } when !(Parameters[^1].ParameterType.GetInterface("IDynamicParameter") is null) => "{}",
                            { HasDefaultValue: true } => $"={parameter.DefaultValue}",
                            _ => ""
                        });
                        infoSb.Append(',');
                    }

                    Information = infoSb.ToString()[..^1];
                }
            }
            else
            {
                Type = MemberType.MethodWithInfo;
                Information = info.Text;
            }
        }

        private TailParameterType TailParameterType { get; }
        private ParameterInfo[] Parameters { get; }
        private int MinParameterCount { get; }
        private int NonArrayParameterCount { get; }
        private int MaxParameterCount { get; }
        private Func<object?, object?[]?, object?> Invoke { get; }

        private bool CanFitTo(int argumentCount)
        => argumentCount >= MinParameterCount
                   && argumentCount <= MaxParameterCount;
        /// <summary>
        /// Execute this object.
        /// </summary>
        /// <param name="args">The arguments for execution.</param>
        /// <returns>TraceBack result of this object.</returns>
        public override TraceBack Execute(string[] args)
        {
            if (!CanFitTo(args.Length)) return TraceBack.ObjectNotFound;
            if (TailParameterType == TailParameterType.NoParameter)
            {
                Invoke(Instance, null);
                return TraceBack.AllOk;
            }

            var pass = new object?[Parameters.Length];
            var i = 0;
            for (; i < NonArrayParameterCount; i++)
                pass[i] = i < args.Length
                    ? (Parameters[i].GetCustomAttribute<MsParserAttribute>(true)
                           ?.Converter
                       ?? (source => source)) //Converter
                    (args[i])
                    : Parameters[i].DefaultValue;

            if (TailParameterType == TailParameterType.Normal)
            {
                Invoke(Instance, pass);
                return TraceBack.AllOk;
            }

            if (TailParameterType == TailParameterType.DynamicParameter)
            {
                var dynamicParameter = Parameters[^1].ParameterType.Assembly
                    .CreateInstance(Parameters[^1].ParameterType.FullName
                                    ?? Parameters[^1].ParameterType.Name) as IDynamicParameter;
                if (dynamicParameter?.Parse(i < args.Length ? args[i..] : null) ?? false)
                {
                    pass[i] = dynamicParameter;
                    Invoke(Instance, pass);
                    return TraceBack.AllOk;
                }

                return TraceBack.InvalidCommand;
            }

            if (i < args.Length)
            {
                var argArray = args[i..];
                var array = Array.CreateInstance(Parameters[^1].ParameterType.GetElementType()
                                                 ?? typeof(string), argArray.Length);
                var convert = Parameters[^1].GetCustomAttribute<MsParserAttribute>(true)?.Converter
                              ?? (source => source);
                var j = 0;
                foreach (var arg in argArray) array.SetValue(convert(arg), j++);
                pass[i] = array;
            }
            else
            {
                pass[i] = Array.CreateInstance(Parameters[^1].ParameterType.GetElementType()
                                               ?? typeof(string), 0);
            }

            Invoke(Instance, pass);
            return TraceBack.AllOk;
        }
    }
}