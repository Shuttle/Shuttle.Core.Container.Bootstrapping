﻿using Moq;
using NUnit.Framework;

namespace Shuttle.Core.Container.Bootstrapping.Tests
{
    public class BootstrapFixture : IComponentRegistryBootstrap, IComponentResolverBootstrap
    {
        private static bool _bootstrapRegisterCalled;
        private static bool _bootstrapResolveCalled;

        [Test]
        public void Should_be_able_to_bootstrap()
        {
            var registry = new Mock<IComponentRegistry>();
            var resolver = new Mock<IComponentResolver>();

            resolver.Setup(m => m.Resolve(typeof(IComponentResolver))).Returns(new ComponentResolverDelegate());

            registry.Object.RegistryBootstrap();
            resolver.Object.ResolverBootstrap();

            Assert.IsTrue(_bootstrapRegisterCalled);
            Assert.IsTrue(_bootstrapResolveCalled);

            registry.Verify(m => m.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Singleton));
            resolver.Verify(m => m.ResolveAll(typeof(ISomeDependency)));
        }

        public void Register(IComponentRegistry registry)
        {
            _bootstrapRegisterCalled = true;

            registry.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Singleton);
        }

        public void Resolve(IComponentResolver resolver)
        {
            _bootstrapResolveCalled = true;

            resolver.ResolveAll(typeof(ISomeDependency));
        }
    }
}