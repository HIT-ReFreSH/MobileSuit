#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PlasticMetal.MobileSuit.IO;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Members;


namespace PlasticMetal.MobileSuit
{
    partial class MobileSuitHost
    {
        public delegate TraceBack BuildInCommand(string[] args);

        private TraceBack EnterObjectMember(string[] args)
        {

            if (WorkType == null || WorkInstance == null || Assembly == null) return TraceBack.InvalidCommand;
            var nextObject = WorkType.GetProperty(args[0], IExecutable.Flags)?.GetValue(WorkInstance) ??
                             WorkType.GetField(args[0], IExecutable.Flags)?.GetValue(WorkInstance);
            InstanceRef.Push(Current);
            var fName = WorkType?.GetProperty(args[0], IExecutable.Flags)?.Name ??
                        WorkType?.GetField(args[0], IExecutable.Flags)?.Name;
            if (fName == null || nextObject == null) return TraceBack.ObjectNotFound;
            InstanceNameStk.Add(fName);
            Current = new MobileSuitObject(nextObject);
            WorkInstanceInit();
            return TraceBack.AllOk;
        }
        private TraceBack LeaveObject(string[] args)
        {
            if (InstanceRef.Count == 0)
                return TraceBack.InvalidCommand;
            if (WorkType == null) return TraceBack.InvalidCommand;
            Current = InstanceRef.Pop();
            InstanceNameStk.RemoveAt(InstanceNameStk.Count - 1);//PopBack
            WorkInstanceInit();
            return TraceBack.AllOk;
        }
        private TraceBack CreateObject(string[] args)
        {
            if (Assembly == null) return TraceBack.InvalidCommand;

            var type = Assembly.GetType(args[0], false, true) ??
            Assembly.GetType(WorkType?.FullName + '.' + args[0], false, true) ??
            Assembly.GetType(Assembly.GetName().Name + '.' + args[0], false, true);
            if (type is null)
            {
                return TraceBack.ObjectNotFound;
            }
            if (type.FullName == null) return TraceBack.InvalidCommand;
            Current = new MobileSuitObject(Assembly.CreateInstance(type.FullName));
            Prompt = (WorkType?.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute
                      ?? new MobileSuitInfoAttribute(args[0])).Prompt;
            InstanceRef.Clear();
            InstanceNameStk.Clear();
            InstanceNameStk.Add($"(new {WorkType?.Name})");
            WorkInstanceInit();
            return TraceBack.AllOk;
        }
        private TraceBack ViewObject(string[] args)
        {
            if (WorkType == null || Assembly == null) return TraceBack.InvalidCommand;

            var obj = WorkType.GetProperty(args[0], IExecutable.Flags)?.GetValue(WorkInstance) ??
                      WorkType.GetField(args[0], IExecutable.Flags)?.GetValue(WorkInstance);
            if (obj == null)
            {
                return TraceBack.ObjectNotFound;
            }
            Io.WriteLine(obj.ToString() ?? "<Unknown>");
            return TraceBack.AllOk;
        }
        private TraceBack SwitchOption(string optionName)
        {
            switch (optionName)
            {
                case "sr":
                case "ShowRef":
                    ShowRef = !ShowRef;
                    return TraceBack.AllOk;
                case "sd":
                case "ShowDone":
                    ShowDone = !ShowDone;
                    return TraceBack.AllOk;

                case "tb":
                case "TraceBack":
                    UseTraceBack = !UseTraceBack;
                    return TraceBack.AllOk;
                default:
                    return TraceBack.InvalidCommand;
            }
        }
        private TraceBack ModifyMember(string[] args)
        {
            if (WorkType == null || Assembly == null) return TraceBack.ObjectNotFound;
            var obj = WorkType?.GetProperty(args[0], IExecutable.Flags) as MemberInfo ?? WorkType?.GetField(args[0], IExecutable.Flags);
            var objProp = WorkType?.GetProperty(args[0], IExecutable.Flags);
            if (obj == null || objProp == null) return TraceBack.MemberNotFound;
            var objSet = (SetProp)objProp.SetValue;

            var cvt = (obj.GetCustomAttribute(typeof(ArgumentConverterAttribute)) as ArgumentConverterAttribute)?.Converter;
            try
            {
                objSet(WorkInstance, cvt != null ? cvt(args[1]) : args[1]);
                return TraceBack.AllOk;
            }
            catch
            {
                return TraceBack.InvalidCommand;
            }
        }
        private TraceBack ListMembers()
        {
            if (Current == null) return TraceBack.InvalidCommand;
            Io.WriteLine("Members:", IoInterface.OutputType.ListTitle);
            Io.AppendWriteLinePrefix();
            foreach (var (name, member) in Current)
            {
                var (infoColor, lChar, rChar) = member.Type switch
                {
                    MemberType.MethodWithInfo => (ConsoleColor.Blue, '[', ']'),
                    MemberType.MethodWithoutInfo => (ConsoleColor.DarkBlue, '(', ')'),
                    MemberType.FieldWithInfo => (ConsoleColor.Green, '<', '>'),
                    _ => (ConsoleColor.DarkGreen, '{', '}')
                };

                Io.WriteLine(contentGroup: new (string, ConsoleColor?)[]
                {
                    (name,null),
                    ($"{lChar}{member.Information}{rChar}",infoColor)
                });
            }

            Io.SubtractWriteLinePrefix();
            return TraceBack.AllOk;
        }
    }
}
