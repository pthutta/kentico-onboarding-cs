using System;
using System.Collections.Generic;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Enums;

namespace TodoApp.Contracts.Containers
{
    public interface IDependencyContainer : IDisposable
    {
        IDependencyContainer RegisterBootstrapper<TBootstrap>()
            where TBootstrap : IBootstrap, new();

        IDependencyContainer RegisterType<TFrom, TTo>(LifetimeManagerType lifetimeManagerType = LifetimeManagerType.SingletonPerRequest)
            where TTo : TFrom;

        IDependencyContainer RegisterType<TTo>(Func<TTo> instanceFactory, LifetimeManagerType lifetimeManagerType = LifetimeManagerType.SingletonPerRequest);

        object Resolve(Type type);

        IEnumerable<object> ResolveAll(Type type);

        IDependencyContainer CreateChildContainer();
    }
}
