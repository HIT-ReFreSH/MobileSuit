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
        private List<Type> Providers { get; } = new();
        protected internal Type GeneratorType { get; set; } = typeof(BasicPromptGenerator);
        protected internal void AddProvider(Type provider)
        {
            Providers.Add(provider);
        }

        protected internal IPromptGenerator Build(IMobileSuitHost host, IIOHub iOHub, object instance)
        {
            var exception = new Exception(Lang.PromptGeneratorBuilder_NoRoute);
            var args = new[] { instance, host, iOHub };
            return GeneratorType.GetConstructor(new[] {typeof(IEnumerable<IPromptProvider>)})
                ?.Invoke(new object?[]
                {
                    Providers.Select(p => p.GetConstructors(BindingFlags.Public).FirstOrDefault())
                        .Select(constructor =>
                            constructor?.Invoke(
                                constructor.GetParameters().Select(
                                    parameter =>
                                        args.FirstOrDefault(arg => parameter.ParameterType.IsInstanceOfType(arg))
                                        ?? throw exception).ToArray() ?? throw exception) as IPromptProvider
                            ?? throw exception)
                }) as IPromptGenerator??throw exception;
        }
    }
}
