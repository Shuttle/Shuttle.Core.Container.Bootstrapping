using System.Collections.Generic;
using System.Reflection;

namespace Shuttle.Core.Container.Bootstrapping
{
    public interface IBootstrapConfiguration
    {
        BootstrapScan Scan { get; }
        IEnumerable<Assembly> Assemblies { get; }
    }
}