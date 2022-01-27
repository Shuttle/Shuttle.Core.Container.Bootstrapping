using System;
using System.Collections.Generic;
using System.Threading;
using Shuttle.Core.Contract;
using Shuttle.Core.Reflection;

namespace Shuttle.Core.Container.Bootstrapping
{
    public static class ComponentRegistryExtensions
    {
        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentRegistryBootstrap` interface and calls the `Register`
        ///     method.
        /// </summary>
        /// <param name="registry">The `IComponentRegistry` instance to register the mapping against.</param>
        public static void RegistryBootstrap(this IComponentRegistry registry)
        {
            RegistryBootstrap(registry, BootstrapSection.GetConfiguration());
        }

        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentRegistryBootstrap` interface and calls the `Register`
        ///     method.
        /// </summary>
        /// <param name="registry">The `IComponentRegistry` instance to register the mapping against.</param>
        /// <param name="bootstrapConfiguration">
        ///     The `IBootstrapConfiguration` instance that contains the bootstrapping
        ///     configuration.
        /// </param>
        public static void RegistryBootstrap(this IComponentRegistry registry, IBootstrapConfiguration bootstrapConfiguration)
        {
            Guard.AgainstNull(registry, nameof(registry));

            var completed = new HashSet<Type>();

            var reflectionService = new ReflectionService();

            foreach (var assembly in bootstrapConfiguration.Assemblies)
            {
                foreach (var type in reflectionService.GetTypesAssignableTo<IComponentRegistryBootstrap>(
                             assembly))
                {
                    if (completed.Contains(type))
                    {
                        continue;
                    }

                    type.AssertDefaultConstructor(string.Format(Container.Resources.DefaultConstructorRequired,
                        "IComponentRegistryBootstrap", type.FullName));

                    ((IComponentRegistryBootstrap)Activator.CreateInstance(type)).Register(registry);

                    completed.Add(type);
                }
            }
        }
    }
}