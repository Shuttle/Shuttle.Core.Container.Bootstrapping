namespace Shuttle.Core.Container.Bootstrapping
{
    public interface IComponentResolverBootstrap
    {
        void Resolve(IComponentResolver resolver);
    }
}