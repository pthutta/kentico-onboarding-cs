using System;
using TodoApp.Contracts.Containers;
using Unity.Lifetime;

namespace TodoApp.Dependency.Extensions
{
    internal static class LifecycleExtensions
    {
        public static LifetimeManager GetLifetimeManager(this Lifecycle lifecycle)
        {
            switch (lifecycle)
            {
                case Lifecycle.SingletonPerApplication:
                    return new ContainerControlledLifetimeManager();

                case Lifecycle.SingletonPerRequest:
                    return new HierarchicalLifetimeManager();

                default:
                    throw new ArgumentOutOfRangeException(nameof(lifecycle), lifecycle, null);
            }
        }
    }
}
