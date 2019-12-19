#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MobileSuit.IO;

namespace MobileSuit
{
    partial class MobileSuitHost
    {
        public delegate TraceBack BuildInCommand(string[] args);

        private TraceBack EnterObjectMember(string[] args)
        {
            
            if (WorkType == null || WorkInstance == null || Assembly == null) return TraceBack.InvalidCommand;
            var nextObject = WorkType.GetProperty(args[0], Flags)?.GetValue(WorkInstance) ??
                             WorkType.GetField(args[0], Flags)?.GetValue(WorkInstance);
            InstanceRef.Push(WorkInstance);
            var fName = WorkType?.GetProperty(args[0], Flags)?.Name ??
                        WorkType?.GetField(args[0], Flags)?.Name;
            if (fName == null || nextObject == null) return TraceBack.ObjectNotFound;
            InstanceNameStk.Add(fName);
            WorkInstance = nextObject;
            WorkType = nextObject.GetType();
            WorkInstanceInit();
            return TraceBack.AllOk;
        }
        private TraceBack LeaveObject(string[] args)
        {
            if (InstanceRef.Count == 0)
                return TraceBack.InvalidCommand;
            if (WorkType == null) return TraceBack.InvalidCommand;
            WorkInstance = InstanceRef.Pop();
            InstanceNameStk.RemoveAt(InstanceNameStk.Count - 1);//PopBack
            WorkType = WorkInstance.GetType();
            WorkInstanceInit();
            return TraceBack.AllOk;
        }
        private TraceBack CreateObject(string[] args)
        {
            if (Assembly == null) return TraceBack.InvalidCommand;
            WorkType =
                Assembly.GetType(args[0], false, true) ??
                Assembly.GetType(WorkType?.FullName + '.' + args[0], false, true) ??
                Assembly.GetType(Assembly.GetName().Name + '.' + args[0], false, true);
            if (WorkType == null)
            {
                return TraceBack.ObjectNotFound;
            }
            if (WorkType?.FullName == null) return TraceBack.InvalidCommand;
            WorkInstance = Assembly.CreateInstance(WorkType?.FullName ?? throw new NullReferenceException());
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

            var obj = WorkType.GetProperty(args[0], Flags)?.GetValue(WorkInstance) ??
                      WorkType.GetField(args[0], Flags)?.GetValue(WorkInstance);
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
            var obj = WorkType?.GetProperty(args[0], Flags) as MemberInfo ?? WorkType?.GetField(args[0], Flags);
            var objProp = WorkType?.GetProperty(args[0], Flags);
            if (obj == null || objProp == null) return TraceBack.MemberNotFound;
            var objSet = (SetProp)objProp.SetValue;

            var cvt = (obj.GetCustomAttribute(typeof(MobileSuitDataConverterAttribute)) as MobileSuitDataConverterAttribute)?.Converter;
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
            if (WorkType == null) return TraceBack.InvalidCommand;
            var fi = (from i in (from f in WorkType.GetFields(Flags)
                                 select (MemberInfo)f).Union
                               (from p in WorkType.GetProperties(Flags)
                                select (MemberInfo)p)
                      orderby i.Name
                      select i).ToList();


            if (fi.Any())
            {
                Io.WriteLine("Members:", IoInterface.OutputType.ListTitle);
                foreach (var item in fi)
                {
                    var info = item.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute;
                    var exInfo = info == null
                                ? ""
                                : $"[{info.Prompt}]";
                    Io.Write($"\t{item.Name}");
                    var otType = info == null
                        ? IoInterface.OutputType.MobileSuitInfo
                        : IoInterface.OutputType.CustomInfo;
                    Io.Write(exInfo, otType);
                    Io.Write("\n");
                }
            }
            var mi = (from m in WorkType.GetMethods(Flags)
                      where
                            !(from p in WorkType.GetProperties(Flags)
                              select $"get_{p.Name}").Contains(m.Name)
                         && !(from p in WorkType.GetProperties(Flags)
                              select $"set_{p.Name}").Contains(m.Name)
                      select m).ToList();


            if (!mi.Any()) return TraceBack.AllOk;
            {
                Io.WriteLine("Methods:", IoInterface.OutputType.ListTitle);
                foreach (var item in mi)
                {
                    var info = item.GetCustomAttribute(typeof(MobileSuitInfoAttribute)) as MobileSuitInfoAttribute;
                    var exInfo = info == null
                        ? $"({ item.GetParameters().Length} Parameters)"
                        : $"[{info.Prompt}]";
                    Io.Write($"\t{item.Name}");
                    var otType = info == null
                        ? IoInterface.OutputType.MobileSuitInfo
                        : IoInterface.OutputType.CustomInfo;
                    Io.Write(exInfo, otType);
                    Io.Write("\n");
                }
            }
            return TraceBack.AllOk;
        }
    }
}
