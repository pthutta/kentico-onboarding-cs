using System;
using System.Collections.Generic;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;

namespace TodoApp.Dependency.Tests.Containers
{
    internal class TestContainer : IDependencyContainer
    {
        private readonly List<Type> _registeredTypes = new List<Type>();

        public IDependencyContainer RegisterBootstrapper<TBootstrap>()
            where TBootstrap : IBootstrap, new()
            => new TBootstrap().RegisterTypes(this);

        public IDependencyContainer RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            _registeredTypes.Add(typeof(TFrom));
            return this;
        }

        public IDependencyContainer RegisterType<TTo>(Func<TTo> objectGetter)
        {
            _registeredTypes.Add(typeof(TTo));
            return this;
        }

        public IEnumerable<Type> GetRegisteredTypes()
            => _registeredTypes;

        public void Dispose()
            => throw new NotImplementedException();

        public object Resolve(Type type)
            => throw new NotImplementedException();

        public IEnumerable<object> ResolveAll(Type type)
            => throw new NotImplementedException();

        public IDependencyContainer CreateChildContainer()
            => throw new NotImplementedException();
    }
}
