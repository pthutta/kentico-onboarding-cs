using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace TodoApp.Api.Resolvers
{
    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer Container;

        public UnityResolver(IUnityContainer container)
        {
            this.Container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = Container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}