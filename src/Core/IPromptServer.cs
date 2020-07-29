namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    ///     represents a server provides prompt output.
    /// </summary>
    public interface IPromptServer
    {
        /// <summary>
        ///     get the default prompt server of Mobile Suit
        /// </summary>
        public static IPromptServer DefaultPromptServer { get; } = new PromptServer();


        /// <summary>
        ///     update the prompt of this prompt server
        /// </summary>
        /// <param name="returnValue">return value of last command</param>
        /// <param name="information">information of current instance</param>
        /// <param name="traceBack">traceBack of last command</param>
        public void Update(string returnValue, string information, TraceBack traceBack);

        /// <summary>
        ///     update the prompt of this prompt server
        /// </summary>
        /// <param name="returnValue">return value of last command</param>
        /// <param name="information">information of current instance</param>
        /// <param name="traceBack">traceBack of last command</param>
        /// <param name="promptInformation">information shows when traceBack==TraceBack.Prompt</param>
        public void Update(string returnValue, string information, TraceBack traceBack, string promptInformation);

        /// <summary>
        ///     Output a prompt to IO.Output
        /// </summary>
        public void Print();
    }
}