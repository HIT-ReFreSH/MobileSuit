using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    /// <summary>
    /// Stands for a CommandLineApplication
    /// </summary>
    public abstract class CommandLineApplication : SuitClient,ICommandLineApplication
    {
        /// <inheritdoc/>
        [SuitIgnore]
        public abstract int SuitStartUp(string[]? args);
    }
    /// <summary>
    /// Stands for a CommandLineApplication with dynamic parameters
    /// </summary>
    /// <typeparam name="TArgument">A dynamic parameter</typeparam>
    public abstract class CommandLineApplication<TArgument> : SuitClient, ICommandLineApplication<TArgument>
        where TArgument : IDynamicParameter
    {
        /// <summary>
        /// Show usage of This Application
        /// </summary>
        [SuitIgnore]
        public abstract void SuitShowUsage();
        ///<inheritdoc/>
        [SuitIgnore]
        public abstract int SuitStartUp(TArgument arg);
        /// <inheritdoc/>
        [SuitIgnore]
        public int SuitStartUp(string[]? args)
        {
            if (args?.Length > 0 && typeof(TArgument).Assembly
                        .CreateInstance(typeof(TArgument).FullName
                                        ?? typeof(TArgument).Name) is TArgument arg && arg.Parse(args))
            {
                return SuitStartUp(arg);
            }
            else
            {
                SuitShowUsage();
                return -1;
            }

        }
    }
}
