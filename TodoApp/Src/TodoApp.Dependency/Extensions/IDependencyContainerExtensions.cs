using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;

namespace TodoApp.Dependency.Extensions
{
    internal static class IDependencyContainerExtensions
    {
        internal static IDependencyContainer RegisterBootstrapper<TBootstrap>(this IDependencyContainer container)
            where TBootstrap : IBootstrap, new()
            => new TBootstrap().RegisterTypes(container);
    }
}
