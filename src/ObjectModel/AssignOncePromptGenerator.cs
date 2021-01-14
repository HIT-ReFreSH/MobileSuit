using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.UI;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// A PromptGenerator, which can only be assigned once.
    /// </summary>
    public interface IAssignOncePromptGenerator:IPromptGenerator,IAssignOnce<IPromptGenerator>
    {
    }

    /// <summary>
    /// A PromptGenerator, which can only be assigned once.
    /// </summary>
    public class AssignOncePromptGenerator : AssignOnce<IPromptGenerator>, IAssignOncePromptGenerator
    {
        /// <inheritdoc/>
        public IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt()
            => Element?.GeneratePrompt() ?? Array.Empty<(string, ConsoleColor?, ConsoleColor?)>();
        /// <inheritdoc/>
        public IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt(Func<IPromptProvider, bool> selector)
        => Element?.GeneratePrompt(selector) ?? Array.Empty<(string, ConsoleColor?, ConsoleColor?)>();
    }
}
