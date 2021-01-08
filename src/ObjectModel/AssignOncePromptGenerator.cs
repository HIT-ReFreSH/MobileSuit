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
        public IAssignOnceIOHub IO => Element?.IO ?? new AssignOnceIOHub();

        /// <inheritdoc/>
        public void Print()
            => Element?.Print();
    }
}
