using System;
using System.Collections.Generic;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Exceptions;
using Unity;
using Unity.Exceptions;
using Unity.Injection;
using Unity.Lifetime;

namespace TodoApp.Dependency.Containers
{
    internal class DependencyContainer : IDependencyContainer
    {
        protected IUnityContainer Container;

        public DependencyContainer(IUnityContainer container)
            => Container = container;

        public IDependencyContainer RegisterBootstrapper<TBootstrap>()
            where TBootstrap : IBootstrap, new()
            => new TBootstrap().RegisterTypes(this);

        public IDependencyContainer RegisterType<TFrom, TTo>() 
            where TTo : TFrom
        {
            Container.RegisterType<TFrom, TTo>(new HierarchicalLifetimeManager());

            return this;
        }

        public IDependencyContainer RegisterType<TTo>(Func<TTo> instanceFactory)
        {
            Container.RegisterType<TTo>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(_ => instanceFactory())
            );

            return this;
        }

        public object Resolve(Type type)
            => ResolveType(() => Container.Resolve(type), type);

        public IEnumerable<object> ResolveAll(Type type)
            => ResolveType(() => Container.ResolveAll(type), type);

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
            => new DependencyContainer(Container.CreateChildContainer());

        public void Dispose()
            => Container.Dispose();
    }
}
