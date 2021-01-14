using System;
using System.Collections.Generic;
using System.Linq;
using PlasticMetal.MobileSuit;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI.PowerLine
{
    /// <summary>
    /// A PowerLine themed prompt generator
    /// </summary>
    public class PowerLineGenerator : PromptGenerator
    {
        /// <summary>
        /// PromptProvider for input DefaultValue.
        /// </summary>
        public class InputDefaultValuePromptProvider : InputDefaultValuePromptProviderBase, IPromptProvider
        {
            /// <summary>
            /// Initialize a TraceBackPromptProvider with colors.
            /// </summary>
            /// <param name="iOHub">IOHub this providing info for.</param>
            public InputDefaultValuePromptProvider(IIOHub iOHub) : base(iOHub)
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
            public InputExpressionPromptProvider(IIOHub iOHub):base(iOHub)
            {
            }
            /// <inheritdoc/>
            public override bool AsLabel => true;


            /// <inheritdoc/>
            public override string Content => $" {InputHelper.Expression ?? ""}";
        }
        /// <summary>
        ///     a lightning ⚡ char
        /// </summary>
        protected const char Lightning = '⚡';
        /// <summary>
        ///     a right arrow  char
        /// </summary>
        protected const char RightArrow = '';

        /// <summary>
        ///     a right triangle  char
        /// </summary>
        protected const char RightTriangle = '';

        /// <summary>
        ///     a cross ⨯ char
        /// </summary>
        protected const char Cross = '⨯';
        /// <inheritdoc/>
        public PowerLineGenerator(IEnumerable<IPromptProvider> providers) : base(providers)
        {
            
        }

        private static bool IsInRange(int i, int count)
        {
            return i < count && i >= 0;
        }
        /// <inheritdoc/>
        public override IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GeneratePrompt(Func<IPromptProvider, bool> selector)
        {
            var pvd = Providers.Where(p=>p.Enabled&&selector(p)).ToList();
            //Power line theme uses inverse Background&Foreground
            var backGrounds = pvd.Select(p => p.ForegroundColor).ToArray();
            var foreGrounds = pvd.Select(p => p.BackgroundColor).ToArray();
            if (pvd.Count > 1)
                for (var i = 0; i < pvd.Count - 1;)
                {
                    if (!pvd[i].AsLabel && backGrounds[i] == null)
                    {
                        for (var radius = 1; ; radius++)
                        {
                            if (IsInRange(i - radius, pvd.Count))
                            {
                                if (pvd[i - radius].AsLabel)
                                {
                                    backGrounds[i] = backGrounds[i - radius];
                                    break;
                                }
                            }

                            if (IsInRange(i + radius, pvd.Count))
                            {
                                if (pvd[i + radius].AsLabel)
                                {
                                    backGrounds[i] = backGrounds[i + radius];
                                    break;
                                }

                                continue;
                            }

                            break;
                        }
                    }

                    i++;
                }

            var r = new List<(string, ConsoleColor?, ConsoleColor?)>();
            if (pvd.Count > 0)
                r.Add((" ", foreGrounds[0] ?? ConsoleColor.White, backGrounds[0]));
            for (var i = 0; i < pvd.Count; i++)
            {
                r.Add(($"{pvd[i].Content}", foreGrounds[i] ?? ConsoleColor.White, backGrounds[i]));

                r.Add((" ", foreGrounds[i]??ConsoleColor.White, backGrounds[i]));
                if (!pvd[i].AsLabel)
                {
                    continue;
                }
                r.Add(($"{RightTriangle} ", backGrounds[i] ?? ConsoleColor.White,
                    IsInRange(i + 1, pvd.Count) ? backGrounds[i + 1] : null));
            }


            return r;

        }
    }
}
