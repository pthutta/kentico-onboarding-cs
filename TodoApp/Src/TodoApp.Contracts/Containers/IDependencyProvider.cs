using System;
using System.Collections.Generic;

namespace TodoApp.Contracts.Containers
{
    public interface IDependencyProvider : IDisposable
    {
        object Resolve(Type type);

        IEnumerable<object> ResolveAll(Type type);

        IDependencyProvider CreateChildProvider();
    }
}