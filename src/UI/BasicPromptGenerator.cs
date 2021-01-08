using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// A basic prompt builder for Mobile Suit.
    /// </summary>
    public class BasicPromptGenerator : PromptGenerator
    {
        /// <inheritdoc></inheritdoc>
        public BasicPromptGenerator(IEnumerable<IPromptProvider> providers) : base(providers)
        {
        }
        /// <inheritdoc></inheritdoc>
        protected override IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt()
        {
            List<(string, ConsoleColor?, ConsoleColor?)> l = new();
            var exclusive = Providers.FirstOrDefault(p => p.Enabled && p.Exclusive);
            if (exclusive != null)
            {
                l.Add((exclusive.AsLabel ? $" [{exclusive.Content}]" : exclusive.Content,
                    exclusive.ForegroundColor, exclusive.BackgroundColor));
            }
            else
            {
                l.AddRange(
                    Providers.Select(provider => (provider.AsLabel ? $" [{provider.Content}]" : provider.Content
                        , provider.ForegroundColor, provider.BackgroundColor)));
            }

            l.Add((" > ", null, null));
            return l;
        }
    }
}
