﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlasticMetal.MobileSuit.Logging {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class LogRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LogRes() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PlasticMetal.MobileSuit.Logging.LogRes", typeof(LogRes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 Log Manager 的本地化字符串。
        /// </summary>
        internal static string Class {
            get {
                return ResourceManager.GetString("Class", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Turn off runtime query 的本地化字符串。
        /// </summary>
        internal static string Disable {
            get {
                return ResourceManager.GetString("Disable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Runtime Query 的本地化字符串。
        /// </summary>
        internal static string Dynamic {
            get {
                return ResourceManager.GetString("Dynamic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Turn on runtime query 的本地化字符串。
        /// </summary>
        internal static string Enable {
            get {
                return ResourceManager.GetString("Enable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Find logs with given args, return result count: -s startTime(yyMMdd HH:mm:ss) -e endTime(yyMMdd HH:mm:ss) -t Type(regex pattern) -m Message(regex pattern) 的本地化字符串。
        /// </summary>
        internal static string Find {
            get {
                return ResourceManager.GetString("Find", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Details 的本地化字符串。
        /// </summary>
        internal static string Info {
            get {
                return ResourceManager.GetString("Info", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Log File At 的本地化字符串。
        /// </summary>
        internal static string LogFileAt {
            get {
                return ResourceManager.GetString("LogFileAt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Off 的本地化字符串。
        /// </summary>
        internal static string Off {
            get {
                return ResourceManager.GetString("Off", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 On 的本地化字符串。
        /// </summary>
        internal static string On {
            get {
                return ResourceManager.GetString("On", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Log Operations{Log:Enter Log Manager;Log On/Off:Turn on/off runtime query;Log F:Find logs;Log S:Get Status} 的本地化字符串。
        /// </summary>
        internal static string Server {
            get {
                return ResourceManager.GetString("Server", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Get status of Logs 的本地化字符串。
        /// </summary>
        internal static string Status {
            get {
                return ResourceManager.GetString("Status", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Time 的本地化字符串。
        /// </summary>
        internal static string Time {
            get {
                return ResourceManager.GetString("Time", resourceCulture);
            }
        }
    }
}