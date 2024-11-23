using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using HitRefresh.MobileSuit.Core.Services;
using HitRefresh.MobileSuit.UI;
using Microsoft.Extensions.DependencyInjection;

namespace HitRefresh.MobileSuit.Core;

/// <summary>
///     Factory to create instance from certain context
/// </summary>
/// <param name="context"></param>
/// <returns></returns>
public delegate object? InstanceFactory(SuitContext context);

/// <summary>
///     General methods to build MobileSuit Core
/// </summary>
public static class SuitBuildUtils
{
    /// <summary>
    ///     Property Flag Key for Specific CMD target
    /// </summary>
    public const string SUIT_COMMAND_TARGET = "suit-cmd-target";

    /// <summary>
    ///     Property Flag Key for task io
    /// </summary>
    public const string SUIT_TASK_FLAG = "suit-io-is-task";

    /// <summary>
    ///     Property Flag Value for Specific CMD target
    /// </summary>
    public const string SUIT_COMMAND_TARGET_APP = "app";

    /// <summary>
    ///     Property Flag Value for Specific CMD target
    /// </summary>
    public const string SUIT_COMMAND_TARGET_HOST = "suit";

    /// <summary>
    ///     Property Flag Key for Specific CMD target
    /// </summary>
    public const string SUIT_COMMAND_TARGET_APP_TASK = "app-task";

    /// <summary>
    ///     Create a factory to create converter
    /// </summary>
    /// <param name="type"></param>
    /// <param name="parserAttribute"></param>
    /// <returns></returns>
    public static Func<SuitContext, Converter<string, object?>> CreateConverterFactory
    (
        Type type,
        SuitParserAttribute? parserAttribute
    )
    {
        return context =>
        {
            if (parserAttribute?.Converter is { } c) return c;
            if (type.GetInterfaces()
                    .FirstOrDefault
                     (
                         t =>
                             t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)
                     ) is { } iCollection)
                type = iCollection.GetGenericArguments()[0];
            if (type.IsAssignableFrom(typeof(string))) return s => s;
            return context.ServiceProvider.GetRequiredService<IParsingService>()
                          .Get
                           (
                               type,
                               parserAttribute?.Name ?? string.Empty
                           );
        };
    }

    /// <summary>
    ///     Fill a Arg with given parameterInfo and arg string
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="arg"></param>
    /// <param name="context"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static object? GetArg(ParameterInfo parameter, string? arg, SuitContext context, out int step)
    {
        if (arg is null || parameter.GetCustomAttribute<SuitInjected>() is not null)
        {
            step = 0;
            if (parameter.ParameterType.IsAssignableFrom(typeof(CancellationToken)))
                return context.CancellationToken.Token;
            if (parameter.ParameterType.IsAssignableFrom(typeof(SuitContext))) return context;
            var service = context.ServiceProvider.GetService(parameter.ParameterType);
            if (service is not null) return service;
            if (arg is null)
            {
                if (parameter.HasDefaultValue) return parameter.DefaultValue;
                throw new FormatException();
            }
        }

        step = 1;
        var converterAttr = parameter.GetCustomAttribute<SuitParserAttribute>(true);
        return CreateConverterFactory(parameter.ParameterType, converterAttr)(context)(arg);
    }

    /// <summary>
    ///     Fill an array Arg with given parameterInfo and args string[]
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="argArray"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static object GetArrayArg(ParameterInfo parameter, IReadOnlyList<string> argArray, SuitContext context)
    {
        var converterAttr = parameter.GetCustomAttribute<SuitParserAttribute>(true);
        var type = parameter.ParameterType.GetElementType()!;
        var array = Array.CreateInstance(type, argArray.Count);
        var convert = CreateConverterFactory(type, converterAttr)(context);
        var k = 0;
        foreach (var arg in argArray) array.SetValue(convert(arg), k++);
        return array;
    }

    /// <summary>
    ///     Get member description of SuitMember
    /// </summary>
    /// <param name="sh"></param>
    /// <returns></returns>
    public static string GetMemberInfo(SuitObjectShell sh)
    {
        var infoSb = new StringBuilder();
        if (sh.MemberCount > 0)
        {
            var i = 0;
            foreach (var sys in sh.Members())
            {
                infoSb.Append(sys.AbsoluteName);
                infoSb.Append
                (
                    sys switch
                    {
                        SuitMethodShell => "()",
                        SuitObjectShell => "{}",
                        _ => ""
                    }
                );
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
        var suitMethodParameterInfo = new SuitMethodParameterInfo();
        var originCount = parameters.Count;
        parameters = parameters.Where(p => p.GetCustomAttribute<SuitInjected>() is null).ToList();
        if (originCount == 0)
        {
            suitMethodParameterInfo.TailParameterType = TailParameterType.NoParameter;
        }
        else
        {
            if (parameters.Count == 0)
                suitMethodParameterInfo.TailParameterType = TailParameterType.Normal;
            else if (parameters[^1].ParameterType.IsArray)
                suitMethodParameterInfo.TailParameterType = TailParameterType.Array;
            else if (parameters[^1].ParameterType.GetInterface("IDynamicParameter") is not null)
                suitMethodParameterInfo.TailParameterType = TailParameterType.DynamicParameter;
            else
                suitMethodParameterInfo.TailParameterType = TailParameterType.Normal;


            suitMethodParameterInfo.MaxParameterCount =
                suitMethodParameterInfo.TailParameterType == TailParameterType.Normal
                    ? parameters.Count
                    : int.MaxValue;
            suitMethodParameterInfo.NonArrayParameterCount =
                suitMethodParameterInfo.TailParameterType == TailParameterType.Normal
                    ? parameters.Count
                    : parameters.Count - 1;
            var i = suitMethodParameterInfo.NonArrayParameterCount - 1;
            for (; i >= 0 && parameters[i].IsOptional; i--) { }

            suitMethodParameterInfo.MinParameterCount = i + 1;
            suitMethodParameterInfo.NonArrayParameterCount =
                suitMethodParameterInfo.TailParameterType == TailParameterType.Normal
                    ? originCount
                    : originCount - 1;
        }

        return suitMethodParameterInfo;
    }

    /// <summary>
    ///     Create instance of certain type according to the context
    /// </summary>
    /// <param name="type"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public static object? CreateInstance(Type type, SuitContext s)
    {
        var tryGet = s.ServiceProvider.GetService(type);
        if (tryGet is not null) return tryGet;
        var constructors = type.GetConstructors();

        foreach (var constructor in constructors)
        {
            if (!constructor.IsPublic) continue;
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0) return constructor.Invoke(null);
            var args = GetArgs(parameters, Array.Empty<string>(), s);
            if (args is not null) return constructor.Invoke(args);
        }

        return null;
    }

    /// <summary>
    ///     Get args using given parameters, arg strings and suit context
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="args"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static object?[]? GetArgs
    (
        IReadOnlyList<ParameterInfo> parameters,
        IReadOnlyList<string> args,
        SuitContext context
    )
    {
        return GetArgs(parameters, GetMethodParameterInfo(parameters), args, context);
    }

    /// <summary>
    ///     Get args using given parameters, arg strings and suit context
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="args"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static object?[]? GetArgs
    (
        IReadOnlyList<ParameterInfo> parameters,
        SuitMethodParameterInfo parameterInfo,
        IReadOnlyList<string> args,
        SuitContext context
    )
    {
        var pass = new object?[parameters.Count];
        var i = 0;
        var j = 0;
        try
        {
            for (; i < parameterInfo.NonArrayParameterCount; i++)
                if (j < args.Count)
                {
                    pass[i] = GetArg(parameters[i], args[j], context, out var step);
                    j += step;
                }
                else
                {
                    pass[i] = GetArg(parameters[i], null, context, out _);
                }

            if (parameterInfo.TailParameterType == TailParameterType.Normal) return pass;

            if (parameterInfo.TailParameterType == TailParameterType.DynamicParameter)
            {
                var dynamicParameter = parameters[^1]
                                      .ParameterType.Assembly
                                      .CreateInstance
                                       (
                                           parameters[^1].ParameterType.FullName
                                        ?? parameters[^1].ParameterType.Name
                                       ) as IDynamicParameter;
                if (!dynamicParameter!.Parse
                    (
                        i < args.Count ? args.Skip(i).ToImmutableArray() : Array.Empty<string>(),
                        context
                    )) return null;
                pass[i] = dynamicParameter;

                return pass;
            }

            if (i < args.Count)
                pass[i] = GetArrayArg(parameters[^1], args.Skip(i).ToImmutableArray(), context);
            else
                pass[i] = Array.CreateInstance
                (
                    parameters[^1].ParameterType.GetElementType()
                 ?? typeof(string),
                    0
                );

            return pass;
        }
        catch (FormatException)
        {
            return null;
        }
    }
}