using System;
using System.Collections.Generic;
using TodoApp.Contracts.Bootstraps;

namespace TodoApp.Contracts.Containers
{
    public interface IDependencyContainer : IDisposable
    {
        IDependencyContainer RegisterBootstrapper<TBootstrap>() where TBootstrap : IBootstrap, new();

        IDependencyContainer RegisterType<TFrom, TTo>() where TTo : TFrom;

        IDependencyContainer RegisterType<TTo>(Func<TTo> instanceFactory);

        object Resolve(Type type);

        IEnumerable<object> ResolveAll(Type type);

        IDependencyContainer CreateChildContainer();
    }
}
