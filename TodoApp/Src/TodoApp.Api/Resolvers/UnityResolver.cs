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
using Unity;
using Unity.Exceptions;

namespace TodoApp.Api.Resolvers
{
    public class UnityResolver : IDependencyResolver
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
            typeof(IModelValidatorCache).FullName,
        };

        protected IUnityContainer Container;

        public UnityResolver(IUnityContainer container)
            => Container = container;

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
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
            catch (ResolutionFailedException)
                when (ExcludedTypes.Contains(serviceType.FullName))
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
            => Container.Dispose();
    }
}