using System;
using System.Resources;
using System.Security.AccessControl;
using System.Threading;

namespace PlasticMetal.MobileSuit;

/// <summary>
///     Stores the information of a member to be displayed.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class SuitInfoAttribute : Attribute
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
/// <summary>
///     Stores the information of a member to be displayed.
/// </summary>
public sealed class SuitInfoAttribute<T> : SuitInfoAttribute
{
    /// <summary>
    ///     Initialize with the information.
    /// </summary>
    /// <param name="text">The information.</param>
    public SuitInfoAttribute(string text) : base(new ResourceManager(typeof(T)).GetString(text, Thread.CurrentThread.CurrentCulture) ?? "")
    {
    }

}