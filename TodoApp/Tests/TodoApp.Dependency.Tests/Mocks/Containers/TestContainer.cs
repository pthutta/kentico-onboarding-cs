using System;
using System.Collections.Generic;
using TodoApp.Contracts.Containers;

namespace TodoApp.Dependency.Tests.Mocks.Containers
{
    internal sealed class TestContainer : IDependencyContainer
    {
        private readonly List<Type> _registeredTypes = new List<Type>();

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

        public IDependencyProvider CreateDependencyProvider()
            => throw new NotImplementedException();

        public void Dispose()
            => throw new NotImplementedException();
    }
}
