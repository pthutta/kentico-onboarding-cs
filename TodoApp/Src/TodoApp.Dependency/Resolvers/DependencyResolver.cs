using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using System.Web.Http.Metadata;
using System.Web.Http.Tracing;
using System.Web.Http.Validation;
using Microsoft.Web.Http.Versioning;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Exceptions;

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
            typeof(IModelValidatorCache),
            typeof(ITraceWriter),
            typeof(IReportApiVersions)
        };

        protected IDependencyContainer Container;

        public DependencyResolver(IDependencyContainer container)
            => Container = container;

        public object GetService(Type serviceType)
            => GetService(() => Container.Resolve(serviceType), serviceType);

        public IEnumerable<object> GetServices(Type serviceType)
            => GetService(() => Container.ResolveAll(serviceType), serviceType) ?? new List<object>();

        private static TResult GetService<TResult>(Func<TResult> resolve, Type serviceType)
            where TResult: class
        {
            try
            {
                return resolve();
            }
            catch (DependencyResolutionFailedException)
                when (IsInExcludedTypes(serviceType))
            {
                return null;
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