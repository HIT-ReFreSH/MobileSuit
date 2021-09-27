using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// Objects that provides information to display prompt.
    /// The objects will be initialized with Dependency Injection.
    /// The following types can be Injected: IIOHub, IMobileSuitHost and target <a>SuitObject</a>s.
    /// They must have exactly one public Constructor.
    /// </summary>
    public interface IPromptProvider
    {
        /// <summary>
        /// Show this part of prompt, or not. 
        /// </summary>
        public bool Enabled { get; }
        /// <summary>
        /// Show this part of prompt as a label(<c>$"[{Content}]"</c> for <c>BasicPromptBuilder</c>).
        /// </summary>
        public bool AsLabel { get; }
        /// <summary>
        /// Attribute information for the PromptProvider.
        /// </summary>
        public IDictionary<string,string> Labels { get; }
        /// <summary>
        /// Content to show in the prompt.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// ForegroundColor for this part of prompt.
        /// </summary>
        public ConsoleColor? ForegroundColor { get; }
        /// <summary>
        /// BackgroundColor for this part of prompt.
        /// </summary>
        public ConsoleColor? BackgroundColor { get; }
    }
}
