using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    ///     a prompt server for mobile suit
    /// </summary>
    public class PromptServer : IPromptServer
    {
        /// <summary>
        ///     Initialize a prompt with GeneralIO
        /// </summary>
        public PromptServer() 
        {
        }






        /// <summary>
        ///     Color setting of this prompt server
        /// </summary>
        protected IColorSetting ColorSetting => IO.ColorSetting;
        /// <summary>
        ///     return value from last update
        /// </summary>
        protected string LastReturnValue { get; set; } = "";

        /// <summary>
        ///     Information from last update
        /// </summary>
        protected string LastInformation { get; set; } = "";

        /// <summary>
        ///     TraceBack from last update
        /// </summary>
        protected TraceBack LastTraceBack { get; set; }

        /// <summary>
        ///     Information shows when TraceBack==Prompt from last update
        /// </summary>
        protected string LastPromptInformation { get; set; } = "";

        /// <inheritdoc />
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
            IO.Write(" " + LastInformation, OutputType.Prompt);
            if (LastTraceBack == TraceBack.Prompt) IO.Write($"[{LastPromptInformation}]", OutputType.Prompt);

            IO.Write(" > ", OutputType.Prompt);
        }

        /// <inheritdoc />
        public IAssignOnceIOServer IO { get; } = new AssignOnceIOServer();
    }
}