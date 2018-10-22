using System;
using System.Collections.Generic;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Enums;

namespace TodoApp.Dependency.Tests.Mocks.Containers
{
    internal sealed class TestContainer : IDependencyContainer
    {
        private readonly List<Type> _registeredTypes = new List<Type>();

        public IDependencyContainer RegisterBootstrapper<TBootstrap>()
            where TBootstrap : IBootstrap, new()
            => new TBootstrap().RegisterTypes(this);

        public IDependencyContainer RegisterType<TFrom, TTo>(Lifecycle lifecycle) where TTo : TFrom
        {
            _registeredTypes.Add(typeof(TFrom));
            return this;
        }

        public IDependencyContainer RegisterType<TTo>(Func<TTo> instanceFactory, Lifecycle lifecycle)
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
