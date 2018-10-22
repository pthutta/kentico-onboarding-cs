using System;
using System.Collections.Generic;
using TodoApp.Contracts.Enums;

namespace TodoApp.Contracts.Containers
{
    public interface IDependencyContainer : IDisposable
    {
        IDependencyContainer RegisterType<TFrom, TTo>(Lifecycle lifecycle)
            where TTo : TFrom;

        IDependencyContainer RegisterType<TTo>(Func<TTo> instanceFactory, Lifecycle lifecycle);

        object Resolve(Type type);

        IEnumerable<object> ResolveAll(Type type);

        IDependencyContainer CreateChildContainer();
    }
}
