using System;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    ///     A read-only container which contains a T object. Once T is put in, it will never change.
    /// </summary>
    /// <typeparam name="T">The type of object contained by AssignOnce. Must be a reference type.</typeparam>
    public interface IAssignOnce<in T>
        where T : class
    {
        /// <summary>
        ///     Assign to the AssignOnce. Throws AlreadyAssignedException if assigned.
        /// </summary>
        /// <param name="t">The value to assign.</param>
        public void Assign(T t);
    }

    /// <summary>
    ///     A read-only container which contains a T object. Once T is put in, it will never change.
    /// </summary>
    /// <typeparam name="T">The type of object contained by AssignOnce. Must be a reference type.</typeparam>
    public abstract class AssignOnce<T> : IAssignOnce<T>
        where T : class
    {
        private bool Assigned { get; set; }

        /// <summary>
        ///     The type of object contained by AssignOnce. Null, if not assigned.
        /// </summary>
        protected T? Element { get; private set; }

        /// <inheritdoc />
        public void Assign(T t)
        {
            if (!Assigned)
            {
                Element = t;
                Assigned = true;
            }
            else
            {
                throw new AlreadyAssignedException<T>(Element);
            }
        }
    }

    /// <summary>
    ///     The exception throws when trying to assign to a assigned AssignOnce.
    /// </summary>
    public class AlreadyAssignedException<T> : Exception
        where T : class
    {
        /// <summary>
        ///     Initialize the exception with AssignOnce's current value
        /// </summary>
        /// <param name="currentValue">Current value of AssignOnce</param>
        public AlreadyAssignedException(T? currentValue)
            : base($"This AssignOnce container has already contained value:{currentValue}")
        {
        }

        /// <inheritdoc />
        public AlreadyAssignedException()
        {
        }

        /// <inheritdoc />
        public AlreadyAssignedException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public AlreadyAssignedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}