using System;
using System.Collections.Generic;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using Unity;
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

        public IDependencyContainer RegisterType<TTo>(Func<TTo> objectGetter)
        {
            Container.RegisterType<TTo>(new HierarchicalLifetimeManager(), new InjectionFactory(_ => objectGetter()));

            return this;
        }

        public object Resolve(Type type)
            => Container.Resolve(type);

        public IEnumerable<object> ResolveAll(Type type)
            => Container.ResolveAll(type);

        public IDependencyContainer CreateChildContainer()
            => new DependencyContainer(Container.CreateChildContainer());

        public void Dispose()
            => Container.Dispose();
    }
}
