using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     Type of a member
    /// </summary>
    public enum MemberType
    {
        /// <summary>
        ///     A Method with customized information
        /// </summary>
        MethodWithInfo = 0,

        /// <summary>
        ///     A Method without customized information
        /// </summary>
        MethodWithoutInfo = -1,

        /// <summary>
        ///     A Field/Property with customized information
        /// </summary>
        FieldWithInfo = 1,

        /// <summary>
        ///     A Field/Property without customized information
        /// </summary>
        FieldWithoutInfo = -2
    }

    /// <summary>
    ///     Represents an entity which can be executed.
    /// </summary>
    public interface ISuitShell
    {
        /// <summary>
        ///     MemberCount of shell. Used to sort.
        /// </summary>
        public int MemberCount { get; }

        /// <summary>
        ///     Type of the member
        /// </summary>
        public MemberType Type { get; }

        /// <summary>
        ///     Execute this object.
        /// </summary>
        /// <param name="context">The arguments for execution.</param>
        /// <returns>Result of executing this object.</returns>
        public Task Execute(SuitContext context);

        /// <summary>
        ///     Detect whether this IExecutable may execute the command.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool MayExecute(IReadOnlyList<string> request);
    }
}