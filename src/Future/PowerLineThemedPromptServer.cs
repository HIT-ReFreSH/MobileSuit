using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PlasticMetal.MobileSuit.IO;

namespace PlasticMetal.MobileSuit.Future
{
    /// <summary>
    /// a power line themed prompt server for mobile suit
    /// </summary>
    public class PowerLineThemedPromptServer : PromptServer
    {
        /// <inheritdoc />
        public PowerLineThemedPromptServer() : base(){}

        /// <inheritdoc />
        private PowerLineThemedPromptServer(IIOServer io) : base(io,IColorSetting.DefaultColorSetting) { }

        /// <inheritdoc />
        public PowerLineThemedPromptServer(ISuitConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Create a mobile suit configuration with power line theme
        /// </summary>
        /// <returns></returns>
        public static ISuitConfiguration CreatePowerLineThemeConfiguration()
        {
            var io = new IOServer();
            var r = new SuitConfiguration(typeof(BuildInCommandServer), io,
                new PowerLineThemedPromptServer(io), IColorSetting.DefaultColorSetting);
            io.Prompt = r.PromptServer;
            return r;
        }

        /// <summary>
        /// a lightning ⚡ char
        /// </summary>
        protected const char Lightning = '⚡';

        /// <summary>
        /// a right arrow  char
        /// </summary>
        protected const char RightArrow = '';

        /// <summary>
        /// a right triangle  char
        /// </summary>
        protected const char RightTriangle = '';
        /// <summary>
        /// a cross ⨯ char
        /// </summary>
        protected const char Cross = '⨯';

        /// <inheritdoc />
        public override void Print()
        {
            var tbColor = IColorSetting.SelectColor(ColorSetting, LastTraceBack > 0 ?
                OutputType.Prompt : LastTraceBack == 0 ? OutputType.AllOk : OutputType.Error);

            var lastColor=LastTraceBack==TraceBack.Prompt? ColorSetting.ListTitleColor : ColorSetting.InformationColor;

            IO.Write(' '+ LastInformation+' ', ColorSetting.DefaultColor, lastColor);

            if (!string.IsNullOrEmpty(LastReturnValue))
            {
                IO.Write(RightTriangle.ToString(CultureInfo.CurrentCulture),
                    lastColor, ColorSetting.CustomInformationColor);
                IO.Write($" {Lang.ReturnValue} {RightArrow} {LastReturnValue} ",
                    ColorSetting.DefaultColor, ColorSetting.CustomInformationColor);
                lastColor = ColorSetting.CustomInformationColor;
            }

            string tbExpression = LastTraceBack switch
            {
                TraceBack.Prompt => LastPromptInformation,
                TraceBack.OnExit => "",
                TraceBack.AllOk => Lang.AllOK,
                TraceBack.InvalidCommand => Lang.InvalidCommand,
                TraceBack.ObjectNotFound => Lang.ObjectNotFound,
                TraceBack.MemberNotFound => Lang.MemberNotFound,
                _ => ""
            };

            IO.Write(RightTriangle.ToString(CultureInfo.CurrentCulture),
                lastColor, tbColor);
            IO.Write($" {tbExpression} ", ColorSetting.DefaultColor, tbColor);
            IO.Write(RightTriangle.ToString(CultureInfo.CurrentCulture) + ' ', tbColor);

        }
    }
}
