using PlasticMetal.MobileSuit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlasticMetal.MobileSuit.ObjectModel;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using PlasticMetal.MobileSuit.Core.Services;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// Factory to create instance from certain context
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public delegate object? InstanceFactory(SuitContext context);
    internal static class SuitBuildTools
    {
        public const string SuitCommandTarget = "suit-cmd-target";
        public const string SuitCommandTargetClient = "client";
        public const string SuitCommandTargetServer = "server";
        public const string SuitAsTask = "suit-as-task";
        public static object? GetArg(ParameterInfo parameter, string? arg, SuitContext context, out int step)
        {

            step = 0;
            if (parameter.ParameterType.IsInstanceOfType(typeof(CancellationToken))) return context.CancellationToken;
            if (parameter.ParameterType.IsInstanceOfType(typeof(SuitContext))) return context;
            var service = context.ServiceProvider.GetService(parameter.ParameterType);
            if (service is not null) return service;
            if (arg is null)
            {
                if(parameter.HasDefaultValue) return parameter.DefaultValue;
                throw new FormatException();
            }
            step = 1;
            var converterAttr = parameter.GetCustomAttribute<SuitParserAttribute>(true);
            if (converterAttr?.Converter is not null) throw new FormatException();
            if (parameter.ParameterType.IsInstanceOfType(typeof(string))) return arg;
            return context.ServiceProvider.GetRequiredService<IParsingService>().Get(parameter.ParameterType,
                converterAttr?.Name ?? string.Empty)(arg);

        }

        public static string GetMemberInfo(SuitObjectShell sh)
        {
            var infoSb = new StringBuilder();
            if (sh.MemberCount > 0)
            {
                var i = 0;
                foreach (var sys in sh.Members)
                {
                    infoSb.Append(sys.AbsoluteName);
                    infoSb.Append(sys switch
                    {
                        SuitMethodShell => "()",
                        SuitObjectShell => "{}",
                        _ => ""
                    });
                    infoSb.Append(',');
                    if (++i <= 5) continue;
                    infoSb.Append("...,");
                    break;
                }

                return infoSb.ToString()[..^1];
            }

            return "";
        }
        public static SuitMethodParameterInfo GetMethodParameterInfo(IReadOnlyList<ParameterInfo> parameters)
        {
            var suitMethodParameterInfo=new SuitMethodParameterInfo();
            if (parameters.Count == 0)
            {
                suitMethodParameterInfo.TailParameterType = TailParameterType.NoParameter;
            }
            else
            {
                if (parameters[^1].ParameterType.IsArray)
                    suitMethodParameterInfo.TailParameterType = TailParameterType.Array;
                else if (parameters[^1].ParameterType.GetInterface("IDynamicParameter") is not null)
                    suitMethodParameterInfo.TailParameterType = TailParameterType.DynamicParameter;
                else
                    suitMethodParameterInfo.TailParameterType = TailParameterType.Normal;


                suitMethodParameterInfo.MaxParameterCount = suitMethodParameterInfo.TailParameterType == TailParameterType.Normal
                    ? parameters.Count
                    : int.MaxValue;
                suitMethodParameterInfo.NonArrayParameterCount = suitMethodParameterInfo.TailParameterType == TailParameterType.Normal
                    ? parameters.Count
                    : parameters.Count - 1;
                var i = suitMethodParameterInfo.NonArrayParameterCount - 1;
                for (; i >= 0 && parameters[i].IsOptional; i--)
                {
                }

                suitMethodParameterInfo.MinParameterCount = i + 1;
            }
            return suitMethodParameterInfo;
        }

        public static object?[]? GetArgs(IReadOnlyList<ParameterInfo> parameters, IReadOnlyList<string> args,
            SuitContext context)
            => GetArgs(parameters, GetMethodParameterInfo(parameters), args, context);
        public static object?[]? GetArgs(IReadOnlyList<ParameterInfo> parameters, SuitMethodParameterInfo parameterInfo, IReadOnlyList<string> args, SuitContext context)
        {
            var pass = new object?[parameters.Count];
            var i = 0;
            var j = 0;
            try
            {
                for (; i < parameterInfo.NonArrayParameterCount; i++)
                {
                    if (j < args.Count)
                    {
                        pass[i] = GetArg(parameters[i], args[j], context, out var step);
                        j += step;
                    }

                    pass[i] = GetArg(parameters[i], null, context, out _);


                }

                if (parameterInfo.TailParameterType == TailParameterType.Normal) return pass;

                if (parameterInfo.TailParameterType == TailParameterType.DynamicParameter)
                {
                    var dynamicParameter = parameters[^1].ParameterType.Assembly
                        .CreateInstance(parameters[^1].ParameterType.FullName
                                        ?? parameters[^1].ParameterType.Name) as IDynamicParameter;
                    if (dynamicParameter?.Parse(i < args.Count ? args.Skip(i).ToImmutableArray() : null) ?? false)
                    {
                        pass[i] = dynamicParameter;

                        return pass;
                    }
                    return null;
                }

                if (i < args.Count)
                {
                    var argArray = args.Skip(i).ToImmutableArray();
                    var array = Array.CreateInstance(parameters[^1].ParameterType.GetElementType()
                                                     ?? typeof(string), argArray.Length);
                    var convert = parameters[^1].GetCustomAttribute<SuitParserAttribute>(true)?.Converter
                                  ?? (source => source);
                    var k = 0;
                    foreach (var arg in argArray) array.SetValue(convert(arg), k++);
                    pass[i] = array;
                }
                else
                {
                    pass[i] = Array.CreateInstance(parameters[^1].ParameterType.GetElementType()
                                                   ?? typeof(string), 0);
                }

                return pass;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
