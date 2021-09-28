using System;

namespace PlasticMetal.MobileSuit.Core.Services
{
    /// <summary>
    /// provides basic info of App.
    /// </summary>
    public interface ISuitAppInfo
    {
        /// <summary>
        /// Name of application.
        /// </summary>
        public string AppName { get; }
        /// <summary>
        /// Arguments for startup.
        /// </summary>
        public string[] StartArgs { get; }
    }
    /// <summary>
    /// provides basic info of App.
    /// </summary>
    public class SuitAppInfo : ISuitAppInfo
    {
        /// <inheritdoc/>
        public string AppName { get; set; } = string.Empty;
        /// <inheritdoc/>
        public string[] StartArgs { get; set; } = Array.Empty<string>();
    }
}
