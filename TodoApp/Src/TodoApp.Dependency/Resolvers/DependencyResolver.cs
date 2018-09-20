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
        private static readonly Type[] ExcludedTypes =
        {
            typeof(IHostBufferPolicySelector),
            typeof(IHttpControllerSelector),
            typeof(IHttpControllerActivator),
            typeof(IHttpActionSelector),
            typeof(IHttpActionInvoker),
            typeof(IContentNegotiator),
            typeof(IExceptionHandler),
            typeof(ModelMetadataProvider),
            typeof(IModelValidatorCache)
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
                when (IsInExcludedTypes(serviceType))
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
                when (IsInExcludedTypes(serviceType))
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

        private static bool IsInExcludedTypes(Type serviceType)
            => Array.Exists(ExcludedTypes, type => type == serviceType);
    }
}