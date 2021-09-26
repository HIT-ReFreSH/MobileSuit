#nullable enable
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.Parsing;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Represents type of the last parameter of a method
    /// </summary>
    public enum TailParameterType
    {
        /// <summary>
        ///     Last parameter exists, and is quite normal.
        /// </summary>
        Normal = 0,

        /// <summary>
        ///     Last parameter is an array.
        /// </summary>
        Array = 1,

        /// <summary>
        ///     Last parameter implements IDynamicParameter.
        /// </summary>
        DynamicParameter = 2,

        /// <summary>
        ///     Last parameter does not exist.
        /// </summary>
        NoParameter = -1
    }

    /// <summary>
    ///     Object's Member which may be a method.
    /// </summary>
    public class ExecutableMember : Member
    {
        /// <summary>
        ///     Initialize an Object's Member with its instance and Method's information.
        /// </summary>
        /// <param name="instance">Instance of Object.</param>
        /// <param name="method">Object's member(Method)'s information</param>
        public ExecutableMember(object? instance, MethodBase method) : base(instance, method)
        {
            if (method == null) throw new Exception();
            InvokeMember = method.Invoke;
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

            var info = method.GetCustomAttribute<SuitInfoAttribute>();
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
                            { } when !(Parameters[^1].ParameterType.GetInterface("IDynamicParameter") is null) =>
                                "{}",
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
        private Func<object?, object?[]?, object?> InvokeMember { get; }

        private async Task<ExecuteResult> Execute(object? instance, object?[]? args)
        {
            try
            {
                var returnValue = InvokeMember(instance, args);
                //Process Task
                if (returnValue is Task task)
                {
                    await task;
                    var result = task.GetType().GetProperty("Result");
                    returnValue = result?.GetValue(task);
                    if (returnValue?.GetType().FullName == "System.Threading.Tasks.VoidTaskResult")
                        returnValue = null;
                }

                return new() { TraceBack = RequestStatus.AllOk, ReturnValue = returnValue };
            }
            catch (TargetInvocationException e)
            {
                return new() { TraceBack = RequestStatus.ApplicationError, ReturnValue = e };
            }
        }

        private bool CanFitTo(int argumentCount)
        {
            return argumentCount >= MinParameterCount
                   && argumentCount <= MaxParameterCount;
        }

        /// <inheritdoc/>
        public override async Task<ExecuteResult> Execute(string[] args, CancellationToken token)
        {
            if (args == null || !CanFitTo(args.Length))
            {
                return new() { TraceBack = RequestStatus.ObjectNotFound };
            }

            if (TailParameterType == TailParameterType.NoParameter) return await Execute(Instance, null);

            var pass = new object?[Parameters.Length];
            var i = 0;
            try
            {
                for (; i < NonArrayParameterCount; i++)
                    pass[i] = Parameters[i].GetType().IsInstanceOfType(token) ? token :
                        i < args.Length
                        ? (Parameters[i].GetCustomAttribute<SuitParserAttribute>(true)
                               ?.Converter
                           ?? (source => source)) //Converter
                        (args[i])
                        : Parameters[i].DefaultValue;

                if (TailParameterType == TailParameterType.Normal) return await Execute(Instance, pass);

                if (TailParameterType == TailParameterType.DynamicParameter)
                {
                    var dynamicParameter = Parameters[^1].ParameterType.Assembly
                        .CreateInstance(Parameters[^1].ParameterType.FullName
                                        ?? Parameters[^1].ParameterType.Name) as IDynamicParameter;
                    if (dynamicParameter?.Parse(i < args.Length ? args[i..] : null) ?? false)
                    {
                        pass[i] = dynamicParameter;

                        return await Execute(Instance, pass);
                    }
                    return new() {TraceBack = RequestStatus.InvalidCommand};
                }

                if (i < args.Length)
                {
                    var argArray = args[i..];
                    var array = Array.CreateInstance(Parameters[^1].ParameterType.GetElementType()
                                                     ?? typeof(string), argArray.Length);
                    var convert = Parameters[^1].GetCustomAttribute<SuitParserAttribute>(true)?.Converter
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

                return await Execute(Instance, pass);
            }
            catch (FormatException e)
            {
                return new() {TraceBack = RequestStatus.CommandParsingFailure, ReturnValue = e};
            }
        }
    }
}