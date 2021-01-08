using System;
using System.Collections.Generic;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    ///     represents a server provides prompt output.
    /// </summary>
    public abstract class PromptGenerator : IPromptGenerator
    {
        /// <summary>
        /// Initialize a PromptBuilder with providers.
        /// </summary>
        /// <param name="providers"></param>
        protected PromptGenerator(IEnumerable<IPromptProvider> providers)
        {
            Providers = providers;
        }

        /// <summary>
        ///     get the default prompt server of Mobile Suit
        /// </summary>
        public static PromptGenerator DefaultPromptServer { get; } = new PromptServer();

        /// <summary>
        /// Objects providing prompt.
        /// </summary>
        protected IEnumerable<IPromptProvider> Providers { get; }



        /// <summary>
        ///     Output a prompt to IO.Output
        /// </summary>
        public void Print()
        {
            IO.Write(GeneratePrompt());
        }
        /// <summary>
        /// Build Prompt with current Prompt providers.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt();
        /// <inheritdoc/>
        public IAssignOnceIOHub IO { get; } = new AssignOnceIOHub();
    }
}