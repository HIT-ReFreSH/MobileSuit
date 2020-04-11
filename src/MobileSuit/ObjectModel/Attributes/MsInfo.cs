using System;
using System.Resources;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit.ObjectModel.Attributes
{
    /// <summary>
    /// Stores the information of a member to be displayed.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class MsInfoAttribute : Attribute, IInfoProvider
    {
        /// <summary>
        /// Initialize with the information.
        /// </summary>
        /// <param name="text">The information.</param>
        public MsInfoAttribute(string text)
        {
            Text = text;
        }
        /// <summary>
        /// Initialize with a resource file's type, and the resource key.
        /// </summary>
        /// <summary lang="zh-CN">
        /// 使用资源文件，和键值初始化一个信息类。
        /// </summary>
        /// <param name="resourceType">Resource file's type</param>
        /// <param name="key">The resource key</param>
        public MsInfoAttribute(Type resourceType, string key)
        {
            Text = new ResourceManager(resourceType).GetString(key);
        }
        /// <summary>
        /// The information.
        /// </summary>
        public string Text { get; }
    }
}