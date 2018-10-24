using System;
using System.Collections.Generic;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Enums;
using TodoApp.Contracts.Exceptions;
using TodoApp.Dependency.LifetimeManagers;
using Unity;
using Unity.Exceptions;
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

        public object Resolve(Type type)
            => ResolveType(() => _container.Resolve(type), type);

        public IEnumerable<object> ResolveAll(Type type)
            => ResolveType(() => _container.ResolveAll(type), type);

        private static TResult ResolveType<TResult>(Func<TResult> resolve, Type type)
            where TResult : class
        {
            try
            {
                return resolve();
            }
            catch (ResolutionFailedException ex)
            {
                throw new DependencyResolutionFailedException($"Failed resolution of {type.FullName}", ex);
            }
        }

        public IDependencyContainer CreateChildContainer()
            => new DependencyContainer(_container.CreateChildContainer());

        public void Dispose()
            => _container.Dispose();
    }
}
