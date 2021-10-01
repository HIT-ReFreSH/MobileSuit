namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Represents type of the last parameter of a method
    /// </summary>
    public enum TailParameterType
    {
        /// <summary>
        ///     Last parameter exists, and is quite normal.
        /// </summary>
        Normal = 0,

        /// <summary>
        ///     Last parameter is an array.
        /// </summary>
        Array = 1,

        /// <summary>
        ///     Last parameter implements IDynamicParameter.
        /// </summary>
        DynamicParameter = 2,

        /// <summary>
        ///     Last parameter does not exist.
        /// </summary>
        NoParameter = -1
    }

    /// <summary>
    ///     Parameter information of a method in MobileSuit
    /// </summary>
    public struct SuitMethodParameterInfo
    {
        /// <summary>
        ///     Type of the last parameter
        /// </summary>
        public TailParameterType TailParameterType { get; set; }

        /// <summary>
        ///     Number of the parameters which can be passed at most.
        /// </summary>
        public int MinParameterCount { get; set; }

        /// <summary>
        ///     Number of the parameters which are neither array nor DynamicParameter
        /// </summary>
        public int NonArrayParameterCount { get; set; }

        /// <summary>
        ///     Number of the parameters which can be passed at least.
        /// </summary>
        public int MaxParameterCount { get; set; }
    }
}