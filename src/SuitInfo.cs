using System;
using System.Resources;
using System.Threading;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    ///     Stores the information of a member to be displayed.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SuitInfoAttribute : Attribute, IInfoProvider
    {
        /// <summary>
        ///     Initialize with the information.
        /// </summary>
        /// <param name="text">The information.</param>
        public SuitInfoAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        ///     Initialize with a resource file's type, and the resource key.
        /// </summary>
        /// <summary lang="zh-CN">
        ///     使用资源文件，和键值初始化一个信息类。
        /// </summary>
        /// <param name="resourceType">Resource file's type</param>
        /// <param name="key">The resource key</param>
        public SuitInfoAttribute(Type resourceType, string key)
        {
            Text = new ResourceManager(resourceType).GetString(key, Thread.CurrentThread.CurrentCulture) ?? "";
        }

        /// <summary>
        ///     The information.
        /// </summary>
        public string Text { get; }
    }
}