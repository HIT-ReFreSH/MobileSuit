using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PlasticMetal.MobileSuit.Core.Services
{
    /// <summary>
    ///     represents a generator provides prompt output.
    /// </summary>
    public delegate IEnumerable<PrintUnit> PromptFormatter(IEnumerable<PrintUnit> origin);

    /// <summary>
    ///     Default prompt formatters.
    /// </summary>
    public static class PromptFormatters
    {
        /// <summary>
        ///     a lightning ⚡ char
        /// </summary>
        public const char Lightning = '\u26A1';

        /// <summary>
        ///     a right arrow  char
        /// </summary>
        public const char RightArrow = '';

        /// <summary>
        ///     a right triangle  char
        /// </summary>
        public const char RightTriangle = '';

        /// <summary>
        ///     a cross ⨯ char
        /// </summary>
        public const char Cross = '⨯';

        /// <summary>
        ///     A basic prompt formatter for Mobile Suit.
        /// </summary>
        public static IEnumerable<PrintUnit> BasicPromptFormatter(IEnumerable<PrintUnit> origin)
        {
            List<PrintUnit> l = new();
            var orgList = origin.ToList();
            for (var i = 0; i < orgList.Count; i++)
            {
                var unit = orgList[i];
                var output = new StringBuilder();
                if (i == 0) output.Append(' ');

                output.Append($"[{unit.Text}] ");
                if (i == orgList.Count - 1) output.Append('>');
                l.Add((output.ToString(), unit.Foreground, unit.Background));
            }

            return l;
        }

        /// <summary>
        ///     A PowerLine themed prompt generator
        /// </summary>
        public static IEnumerable<PrintUnit> PowerLineFormatter(IEnumerable<PrintUnit> origin)
        {
            var orgList = origin.ToList();
            //Power line theme uses inverse Background&Foreground
            var backGrounds = orgList.Select(p => p.Foreground ?? Color.Black).ToArray();
            var foreGrounds = orgList.Select(p => p.Background).ToArray();
            for (var i = 0; i < orgList.Count; i++)
            {
                if (foreGrounds[i] is not null) continue;
                var bg = backGrounds[i];
                foreGrounds[i] = bg.R <= 0x7F || bg.G <= 0x7F || bg.B <= 0x7F ? Color.White : Color.Black;
            }

            var r = new List<PrintUnit>();
            if (orgList.Count > 0)
                r.Add((" ", foreGrounds[0], backGrounds[0]));
            for (var i = 0; i < orgList.Count; i++)
            {
                var txt = orgList[i].Text;
                if (txt.StartsWith(Lang.Tasks))
                    txt = txt.Replace(Lang.Tasks, $"{Lightning} ", StringComparison.CurrentCulture);
                r.Add(($"{txt}", foreGrounds[i], backGrounds[i]));
                r.Add((" ", foreGrounds[i], backGrounds[i]));
                r.Add(($"{RightTriangle} ", backGrounds[i],
                    i + 1 < orgList.Count ? backGrounds[i + 1] : null));
            }


            return r;
        }
    }
}