using System;
using TodoApp.Contracts.Enums;
using Unity.Lifetime;

namespace TodoApp.Dependency.LifetimeManagers
{
    internal static class LifetimeManagerTypesExtensions
    {
        public static LifetimeManager GetLifetimeManager(this LifetimeManagerType lifetimeManagerType)
        {
            switch (lifetimeManagerType)
            {
                case LifetimeManagerType.SingletonPerApplication:
                    return new ContainerControlledLifetimeManager();

                case LifetimeManagerType.SingletonPerRequest:
                    return new HierarchicalLifetimeManager();

                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetimeManagerType), lifetimeManagerType, null);
            }
        }
    }
}
