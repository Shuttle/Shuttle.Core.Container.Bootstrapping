
# Shuttle.Core.Container.Bootstrapping

```
PM> Install-Package Shuttle.Core.Container.Bootstrapping
```

This package is an extension for `Shuttle.Core.Container` and provides a way to automatically register and resolve specified components.

It is important to note that your bootstrap implementations should be idempotent as they *may* be called more than once in some instances.

You can control the bootstrapping behaviour using and implementation of the `IBootstrapConfiguration`.  The `BootstrapSection` can provide a default `BootstrapConfiguration` instance for the following application configuration options:

``` xml
<configuration>
  <configSections>
    <section name="bootstrap" type="Shuttle.Core.Container.Bootstrapping.BootstrapSection, Shuttle.Core.Container.Bootstrapping"/>
  </configSections>

  <bootstrap scan="All|Shuttle|None">
    <assemblies>
      <add name="Shuttle.Module1" />
      <add name="Shuttle.Module2" />
    </assemblies>
  </bootstrap>
</configuration>
```

In addition a `ComponentResolver` implementation of the `IComponentResolver` interface is registered when using bootstrapping.  This is a proxy to the actual implementation of the `IComponentResolver` interface and forwards the relevant calls to the assigned instance.  

***Note***:  Avoid using the service locator anti-pattern by injecting `IComponentResolver` in order to obtain singleton services.  Instead the intention is to use the `IComponentResolver` as a factory for transient instances where creating another interface may be superfluous.

<a name="IComponentRegistryBootstrap"></a>

### IComponentRegistryBootstrap

You can call the `IComponentRegistry.RegistryBootstrap()` extension method to bootstrap registrations.  This method will instance any classes that implement the `IComponentRegistryBootstrap` interface and call the `Register(IComponentRegistry registry)` method within that instance.  The implementation has to have a default constructor.

Example:

```c#
public class Bootstrap : IComponentRegistryBootstrap
{
    private static bool _registryBootstrapCalled;

    public void Register(IComponentRegistry registry)
    {
        Guard.AgainstNull(registry, nameof(registry));

        if (_registryBootstrapCalled)
        {
            return;
        }

        if (!registry.IsRegistered<IDependencyType>())
        {
            registry.AttemptRegisterInstance(ImplementationInstance));
        }

        registry.AttemptRegister<IAnotherDependency, ImplementationType>();

        _registryBootstrapCalled = true;
    }
}
```

You may also make use of the registry's configuration section to specify explicit registrations and to disable scanning of assemblies (which defaults to `true`):

``` xml
<configuration>
  <configSections>
    <section name="componentRegistry" type="Shuttle.Core.Container.Bootstrapping.ComponentRegistrySection, Shuttle.Core.Container.Bootstrapping"/>
  </configSections>

  <componentRegistry scan="true|false">
    <components>
      <add dependencyType="Shuttle.Module1" />
      <add dependencyType="Shuttle.Module2" />
    </components>
    <collections>
      <collection dependencyType="IPlugin">
        <add implementationType="Plugin1" />
        <add implementationType="Plugin2" />
        <add implementationType="Plugin3" />
      </collection>
    </collections>
  </componentRegistry>
</configuration>
```

<a name="IComponentResolverBootstrap"></a>

### IComponentResolverBootstrap

You can call the `IComponentResolver.ResolverBootstrap()` extension method to bootstrap resolving components.  This method will instance any classes that implement the `IComponentResolverBootstrap` interface and call the `Resolve(IComponentResolver resolver)` method within that instance.  The implementation has to have a default constructor.

Example:

```c#
public class Bootstrap : IComponentResolverBootstrap
{
    private static bool _resolverBootstrapCalled;

    public void Resolve(IComponentResolver resolver)
    {
        Guard.AgainstNull(resolver, nameof(resolver));

        if (_resolverBootstrapCalled)
        {
            return;
        }

        resolver.Resolve<IDependencyType>();

        _resolverBootstrapCalled = true;
    }
}
```

You may be wondering why one would need to resolve instances.  If you happen to have an instance that has dependencies but is not a dependency itself then you would need to resolve it in order to inject the relevant implementations.  This is particularly useful in framework/plug-in architectures. 

In addition you may also use the following configuration section to specify explicit resolutions and to disable scanning of assemblies (which defaults to `true`):

``` xml
<configuration>
  <configSections>
    <section name="componentResolver" type="Shuttle.Core.Container.Bootstrapping.ComponentResolverSection, Shuttle.Core.Container.Bootstrapping"/>
  </configSections>

  <componentResolver scan="true|false">
    <components>
      <add dependencyType="Shuttle.Module1" />
      <add dependencyType="Shuttle.Module2" />
    </components>
  </componentResolver>
</configuration>
```
