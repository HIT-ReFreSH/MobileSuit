using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI
{
    /// <summary>
    /// A builder to build prompt generator.
    /// </summary>
    public class PromptGeneratorBuilder
    {
        private List<(Type, Delegate?)> Providers { get; } = new();
        /// <summary>
        /// Type of the Prompt Generator
        /// </summary>
        protected internal Type GeneratorType { get; set; } = typeof(BasicPromptGenerator);
        /// <summary>
        /// Add a PromptProvider, which will be initialized with constructor.
        /// </summary>
        /// <param name="provider">Type of the PromptProvider, will be initialized with constructor.</param>
        protected internal void AddProvider(Type provider)
        {

            Providers.Add((provider, null));
        }
        /// <summary>
        /// Add a PromptProvider, which will be initialized with given delegate.
        /// </summary>
        /// <param name="provider">Type of the PromptProvider, will be initialized with given delegate.</param>
        /// <param name="builder">Delegate to initialize the PromptProvider.</param>
        protected internal void AddProvider(Type provider, Delegate builder)
        {

            Providers.Add((provider, builder));
        }
        /// <summary>
        /// Buildup the Generator, with current environment.
        /// </summary>
        /// <param name="host">Mobile Suit Host</param>
        /// <param name="iOHub">IO Hub</param>
        /// <param name="instance">Instance to drive.</param>
        /// <returns>the Generator</returns>
        protected internal IPromptFormatter Build(IMobileSuitHost host, IIOHub iOHub, object instance)
        {
            var exception = new Exception(Lang.PromptGeneratorBuilder_NoRoute);
            var args = new[] { instance, host, iOHub, iOHub.ColorSetting };
            object?[] MapPara(MethodBase mb) =>
                mb.GetParameters().Select(
                    parameter =>
                        args.FirstOrDefault(arg => parameter.ParameterType.IsInstanceOfType(arg))
                        ?? throw exception).ToArray();
            return GeneratorType.GetConstructor(new[] { typeof(IEnumerable<IPromptProvider>) })
                ?.Invoke(new object?[]
                {
                    Providers.Select(p =>
                    {
                        (Type type,var @delegate)=p;
                        if (@delegate != null)
                            return @delegate.GetMethodInfo().Invoke(@delegate.Target,
                                MapPara(@delegate.Method) ?? throw exception)as IPromptProvider??throw exception;
                        var cons =
                            type.GetConstructors().FirstOrDefault() ?? throw exception;

                        return cons.Invoke(MapPara(cons)) as IPromptProvider??throw exception;
                    }).ToArray()
                }) as IPromptFormatter??throw exception;
        }
}
}
