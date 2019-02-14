using System;
using System.Collections.Generic;
using TodoApp.Contracts.Containers;
using Unity;
using Unity.Exceptions;

namespace TodoApp.Dependency.Containers
{
    internal sealed class DependencyProvider : IDependencyProvider
    {
        private readonly IUnityContainer _container;

        public DependencyProvider(IUnityContainer container)
            => _container = container;

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

        public IDependencyProvider CreateChildProvider()
            => new DependencyProvider(_container.CreateChildContainer());

        public void Dispose()
            => _container.Dispose();
    }
}
