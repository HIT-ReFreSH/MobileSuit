using System;
using System.Collections.Generic;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    ///     represents a generator provides prompt output.
    /// </summary>
    public interface IPromptGenerator
    {
        /// <summary>
        ///     Generate output tuple-array for the prompt.
        /// </summary>
        IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt();
        /// <summary>
        ///     Generate output tuple-array for the prompt, with given selector.
        /// </summary>
        /// <param name="selector">Selector to select the providers to output.</param>
        IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt(Func<IPromptProvider, bool> selector);
    }
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
        public static PromptGenerator DefaultPromptServer { get; } = new BasicPromptGenerator(Array.Empty<IPromptProvider>());

        /// <summary>
        /// Objects providing prompt.
        /// </summary>
        protected IEnumerable<IPromptProvider> Providers { get; }

        /// <inheritdoc/>
        public abstract IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt();

        /// <inheritdoc/>
        public abstract IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt(Func<IPromptProvider,bool> selector);

    }
}