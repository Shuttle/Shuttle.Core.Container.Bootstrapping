using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Reflection;

namespace Shuttle.Core.Container.Bootstrapping
{
    public static class ComponentResolverExtensions
    {
        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentResolverBootstrap` interface and calls the `Resolve`
        ///     method.
        /// </summary>
        /// <param name="resolver">The `IComponentResolver` instance to resolve dependencies from.</param>
        public static void ResolverBootstrap(this IComponentResolver resolver)
        {
            ResolverBootstrap(resolver, BootstrapSection.GetConfiguration());
        }

        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentResolverBootstrap` interface and calls the `Resolve`
        ///     method.
        /// </summary>
        /// <param name="resolver">The `IComponentResolver` instance to resolve dependencies from.</param>
        /// <param name="bootstrapConfiguration">The `IBootstrapConfiguration` instance that contains the bootstrapping configuration.</param>
        public static void ResolverBootstrap(IComponentResolver resolver, IBootstrapConfiguration bootstrapConfiguration)
        {
            Guard.AgainstNull(resolver, nameof(resolver));

            var reflectionService = new ReflectionService();

            foreach (var assembly in bootstrapConfiguration.Assemblies)
            {
                foreach (var type in reflectionService.GetTypesAssignableTo<IComponentResolverBootstrap>(assembly))
                {
                    type.AssertDefaultConstructor(string.Format(Container.Resources.DefaultConstructorRequired,
                        "IComponentResolverBootstrap", type.FullName));

                    ((IComponentResolverBootstrap)Activator.CreateInstance(type)).Resolve(resolver);
                }
            }
        }
    }
}