namespace Shuttle.Core.Container.Bootstrapping
{
    public interface IComponentRegistryBootstrap
    {
        void Register(IComponentRegistry registry);
    }
}