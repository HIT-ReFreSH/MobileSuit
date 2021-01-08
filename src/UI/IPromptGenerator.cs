using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    ///     represents a generator provides prompt output.
    /// </summary>
    public interface IPromptGenerator:IIOInteractive
    {
        /// <summary>
        ///     Output a prompt to IO.Output
        /// </summary>
        public void Print();
    }
}
