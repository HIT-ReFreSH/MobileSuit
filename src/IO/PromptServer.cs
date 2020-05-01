using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    /// a prompt server for mobile suit
    /// </summary>
    public class PromptServer : IPromptServer
    {
        /// <summary>
        /// IO server of this prompt server
        /// </summary>
        protected IIOServer IO { get; }
        /// <summary>
        /// Color setting of this prompt server
        /// </summary>
        protected IColorSetting ColorSetting { get; }

        /// <summary>
        /// Initialize a prompt with GeneralIO
        /// </summary>
        public PromptServer() : this(IIOServer.GeneralIO, IColorSetting.DefaultColorSetting)
        {

        }

        /// <summary>
        /// Initialize a prompt with IO and color setting.
        /// </summary>
        /// <param name="io">io</param>
        /// <param name="colorSetting">color setting</param>
        protected PromptServer(IIOServer? io, IColorSetting? colorSetting)
        {
            IO = io ?? IIOServer.GeneralIO;
            ColorSetting = colorSetting?? IColorSetting.DefaultColorSetting;
        }

        /// <summary>
        /// Initialize a prompt Server with given configuration
        /// </summary>
        /// <param name="configuration"></param>
        public PromptServer(ISuitConfiguration configuration)
            : this(configuration?.IO,
            configuration?.ColorSetting)
        {


        }

        /// <summary>
        /// return value from last update
        /// </summary>
        protected string LastReturnValue { get; set; } = "";

        /// <summary>
        /// Information from last update
        /// </summary>
        protected string LastInformation { get; set; } = "";

        /// <summary>
        ///TraceBack from last update
        /// </summary>
        protected TraceBack LastTraceBack { get; set; }

        /// <summary>
        ///Information shows when TraceBack==Prompt from last update
        /// </summary>
        protected string LastPromptInformation { get; set; } = "";
        ///<inheritdoc/>
        public virtual void Update(string returnValue, string information, TraceBack traceBack)
        {
            LastReturnValue = returnValue;
            LastInformation = information;
            LastTraceBack = traceBack;
        }

        /// <inheritdoc />
        public void Update(string returnValue, string information, TraceBack traceBack, string promptInformation)
        {
            Update(returnValue, information, traceBack);
            LastPromptInformation = promptInformation;

        }

        /// <inheritdoc />
        public virtual void Print()
        {
            IO.Write(LastInformation, OutputType.Prompt);
            if (LastTraceBack == TraceBack.Prompt)
            {
                IO.Write($"[{Lang.Default}: {LastPromptInformation}]", OutputType.Prompt);
            }

            IO.Write(" > ", OutputType.Prompt);
        }
    }
}
