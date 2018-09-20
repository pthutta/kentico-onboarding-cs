using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;
using TodoApp.Contracts.Containers;
using TodoApp.Dependency.Exceptions;

namespace TodoApp.Dependency.Resolvers
{
    internal class DependencyResolver : IDependencyResolver
    {
        private static readonly List<string> ExcludedTypes = new List<string>
        {
            typeof(IHostBufferPolicySelector).FullName,
            typeof(IHttpControllerSelector).FullName,
            typeof(IHttpControllerActivator).FullName,
            typeof(IHttpActionSelector).FullName,
            typeof(IHttpActionInvoker).FullName,
            typeof(IContentNegotiator).FullName,
            typeof(IExceptionHandler).FullName,
            typeof(ModelMetadataProvider).FullName,
            typeof(IModelValidatorCache).FullName
        };

        protected IDependencyContainer Container;

        public DependencyResolver(IDependencyContainer container)
            => Container = container;

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (DependencyResolutionFailedException)
                when (ExcludedTypes.Contains(serviceType.FullName))
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
            catch (DependencyResolutionFailedException)
                when (ExcludedTypes.Contains(serviceType.FullName))
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = Container.CreateChildContainer();
            return new DependencyResolver(child);
        }

        public void Dispose()
            => Container.Dispose();
    }
}