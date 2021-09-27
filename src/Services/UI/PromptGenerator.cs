using System;
using System.Collections.Generic;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    ///     represents a generator provides prompt output.
    /// </summary>
    public interface IPromptFormatter
    {
        /// <summary>
        ///     Generate output tuple-array for the prompt.
        /// </summary>
        IEnumerable<PrintUnit> FormatPrompt(IEnumerable<PrintUnit> origin);

        /// <summary>
        ///     Generate output tuple-array for the prompt, with given selector.
        /// </summary>
        /// <param name="selector">Selector to select the providers to output.</param>
        IEnumerable<PrintUnit> GeneratePrompt(Func<IPromptProvider, bool> selector);
    }
    /// <summary>
    ///     represents a server provides prompt output.
    /// </summary>
    public abstract class PromptGenerator : IPromptFormatter
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
        public IEnumerable<PrintUnit> FormatPrompt()
            => GeneratePrompt(_ => true);

        /// <inheritdoc/>
        public abstract IEnumerable<PrintUnit> GeneratePrompt(Func<IPromptProvider, bool> selector);

    }
}