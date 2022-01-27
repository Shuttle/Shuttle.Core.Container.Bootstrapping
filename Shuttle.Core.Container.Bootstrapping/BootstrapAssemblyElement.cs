using System.Configuration;

namespace Shuttle.Core.Container.Bootstrapping
{
    public class BootstrapAssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => (string) this["name"];
    }
}