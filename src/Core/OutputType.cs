namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    ///     Type of content that writes to the output stream.
    /// </summary>
    public enum OutputType
    {
        /// <summary>
        ///     Normal content.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Prompt content.
        /// </summary>
        Prompt = 1,

        /// <summary>
        ///     Error content.
        /// </summary>
        Error = 2,

        /// <summary>
        ///     All-Ok content.
        /// </summary>
        AllOk = 3,

        /// <summary>
        ///     Title of a list.
        /// </summary>
        ListTitle = 4,

        /// <summary>
        ///     Normal information.
        /// </summary>
        CustomInfo = 5,

        /// <summary>
        ///     Information provided by MobileSuit.
        /// </summary>
        MobileSuitInfo = 6
    }
}