using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// A basic prompt builder for Mobile Suit.
    /// </summary>
    public class BasicPromptGenerator : PromptGenerator
    {
        /// <summary>
        /// PromptProvider for input DefaultValue.
        /// </summary>
        public class InputDefaultValuePromptProvider : InputDefaultValuePromptProviderBase,IPromptProvider
        {
            /// <summary>
            /// Initialize a TraceBackPromptProvider with colors.
            /// </summary>
            /// <param name="iOHub">IOHub this providing info for.</param>
            public InputDefaultValuePromptProvider(IIOHub iOHub):base(iOHub)
            {
            }

            /// <inheritdoc/>
            public override bool AsLabel => true;

            /// <inheritdoc/>
            public override string Content => InputHelper.DefaultInput ?? "";
        }
        /// <summary>
        /// PromptProvider for input Expression.
        /// </summary>
        public class InputExpressionPromptProvider : InputExpressionPromptProviderBase, IPromptProvider
        {
            /// <summary>
            /// Initialize a TraceBackPromptProvider with colors.
            /// </summary>
            /// <param name="iOHub">IOHub this providing info for.</param>
            public InputExpressionPromptProvider(IIOHub iOHub) : base(iOHub)
            {
            }
            /// <inheritdoc/>
            public override bool AsLabel => false;


            /// <inheritdoc/>
            public override string Content => $" {InputHelper.Expression ?? ""}";
        }
        /// <inheritdoc></inheritdoc>
        public BasicPromptGenerator(IEnumerable<IPromptProvider> providers) : base(providers)
        {
            
        }

        /// <inheritdoc></inheritdoc>
        public override IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt(Func<IPromptProvider, bool> selector)
        {
            List<(string, ConsoleColor?, ConsoleColor?)> l = new();

            l.AddRange(
                Providers.Where(p => p.Enabled && selector(p))
                    .Select(provider => (provider.AsLabel ? $" [{provider.Content}]" : provider.Content
                    , provider.ForegroundColor, provider.BackgroundColor)));


            l.Add((" > ", null, null));
            return l;
        }
    }
}
