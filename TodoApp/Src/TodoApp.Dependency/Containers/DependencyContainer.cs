using System;
using TodoApp.Contracts.Containers;
using TodoApp.Dependency.Extensions;
using Unity;
using Unity.Injection;

namespace TodoApp.Dependency.Containers
{
    internal sealed class DependencyContainer : IDependencyContainer
    {
        private readonly IUnityContainer _container;

        public DependencyContainer(IUnityContainer container)
            => _container = container;

        public IDependencyContainer RegisterType<TFrom, TTo>(Lifecycle lifecycle) 
            where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(lifecycle.GetLifetimeManager());

            return this;
        }

        public IDependencyContainer RegisterType<TTo>(Func<TTo> instanceFactory, Lifecycle lifecycle)
        {
            _container.RegisterType<TTo>(
                lifecycle.GetLifetimeManager(),
                new InjectionFactory(_ => instanceFactory())
            );

            return this;
        }

        public IDependencyProvider CreateDependencyProvider()
            => new DependencyProvider(_container);

        public void Dispose()
            => _container.Dispose();
    }
}
