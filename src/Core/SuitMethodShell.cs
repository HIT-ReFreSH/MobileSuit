#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core;

/// <summary>
///     Object's Member which may be a method.
/// </summary>
public class SuitMethodShell : SuitShell
{
    private readonly SuitMethodParameterInfo _suitMethodParameterInfo;

    private SuitMethodShell(MethodBase method, InstanceFactory factory, string? absoluteName = null) : base(method,
        factory, absoluteName)
    {
        if (method == null) throw new Exception();
        InvokeMember = method.Invoke;
        Parameters = method.GetParameters();

        _suitMethodParameterInfo = SuitBuildUtils.GetMethodParameterInfo(Parameters);

        var info = method.GetCustomAttribute<SuitInfoAttribute>();
        if (info is null)
        {
            Type = MemberType.MethodWithoutInfo;
            var infoSb = new StringBuilder();
            if (_suitMethodParameterInfo.MaxParameterCount > 0)
            {
                foreach (var parameter in Parameters)
                {
                    infoSb.Append(parameter.Name);
                    infoSb.Append(parameter switch
                    {
                        { } when parameter.ParameterType.IsArray => "[]",
                        { } when Parameters[^1].ParameterType.GetInterface("IDynamicParameter") is not null =>
                            "{}",
                        {HasDefaultValue: true} => $"={parameter.DefaultValue}",
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

    private ParameterInfo[] Parameters { get; }
    private Func<object?, object?[]?, object?> InvokeMember { get; }

    /// <inheritdoc />
    public override int MemberCount => Parameters.Length;

    /// <summary>
    ///     Create a method shell from delegate
    /// </summary>
    /// <param name="methodName">Name of this command</param>
    /// <param name="delegate"></param>
    /// <returns></returns>
    public static SuitMethodShell FromDelegate(string methodName, Delegate @delegate)
    {
        return new SuitMethodShell(@delegate.Method, _ => @delegate.Target, methodName);
    }

    /// <summary>
    ///     Initialize an Object's Member with its instance and Method's information.
    /// </summary>
    /// <param name="method">Object's member(Method)'s information</param>
    /// <param name="factory"></param>
    public static SuitMethodShell FromInstance(MethodBase method, InstanceFactory factory)
    {
        return new SuitMethodShell(method, factory);
    }

    /// <inheritdoc />
    public override bool MayExecute(IReadOnlyList<string> request)
    {
        return request.Count > 0 && FriendlyNames.Contains(request[0], StringComparer.OrdinalIgnoreCase)
                                 && CanFitTo(request.Count - 1);
    }

    private async Task Execute(SuitContext context, object?[]? args)
    {
        try
        {
            var returnValue = InvokeMember(GetInstance(context), args);
            //Process Task
            if (returnValue is Task task)
            {
                await task;
                var result = task.GetType().GetProperty("Result");
                returnValue = result?.GetValue(task);
                if (returnValue?.GetType().FullName == "System.Threading.Tasks.VoidTaskResult")
                    returnValue = null;
            }

            if (returnValue is RequestStatus status)
            {
                context.Status = status;
                context.Response = null;
            }
            else
            {
                context.Status = RequestStatus.Handled;
                context.Response = returnValue?.ToString();
            }
        }
        catch (TargetInvocationException e)
        {
            context.Status = RequestStatus.Faulted;
            throw e.InnerException ?? e;
        }
        catch (Exception ex)
        {
            context.Status = RequestStatus.Faulted;
            throw ex;
        }
    }

    private bool CanFitTo(int argumentCount)
    {
        return argumentCount >= _suitMethodParameterInfo.MinParameterCount
               && argumentCount <= _suitMethodParameterInfo.MaxParameterCount;
    }

    /// <inheritdoc />
    public override async Task Execute(SuitContext context)
    {
        if (_suitMethodParameterInfo.TailParameterType == TailParameterType.NoParameter)
        {
            await Execute(context, null);
            return;
        }


        var args = context.Request[1..];
        try
        {
            var pass = SuitBuildUtils.GetArgs(Parameters, _suitMethodParameterInfo, args, context);
            if (pass is null) return;
            await Execute(context, pass);
        }
        catch (FormatException)
        {
        }
    }
}