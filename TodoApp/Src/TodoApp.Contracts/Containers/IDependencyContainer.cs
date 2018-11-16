using System;

namespace TodoApp.Contracts.Containers
{
    public interface IDependencyContainer : IDisposable
    {
        IDependencyContainer RegisterType<TFrom, TTo>(Lifecycle lifecycle)
            where TTo : TFrom;

        IDependencyContainer RegisterType<TTo>(Func<TTo> instanceFactory, Lifecycle lifecycle);

        IDependencyProvider CreateDependencyProvider();
    }
}
