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
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Exceptions;
using TodoApp.Dependency.Resolvers;

namespace TodoApp.Dependency.Tests.Resolvers
{
    [TestFixture]
    public class DependencyResolverTests
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

        private IDependencyResolver _resolver;
        private IDependencyContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = Substitute.For<IDependencyContainer>();
            _resolver = new DependencyResolver(_container);
        }

        [Test]
        public void GetService_RegisteredType_ReturnsObject()
        {
            var testString = "Test string";
            var type = typeof(string);
            _container.Resolve(type).Returns(testString);

            var resolvedString = (string)_resolver.GetService(type);

            _container.Received().Resolve(type);
            Assert.That(resolvedString, Is.EqualTo(testString));
        }

        [Test]
        public void GetService_ExcludedTypes_ReturnsNull()
        {
            foreach (var excludedType in ExcludedTypes)
            {
                var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
                _container.Resolve(excludedType).Throws(exception);

                var resolvedObject = _resolver.GetService(excludedType);

                Assert.That(resolvedObject, Is.Null);
            }
        }

        [Test]
        public void GetService_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var type = typeof(IDependencyResolver);
            var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
            _container.Resolve(type).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _resolver.GetService(type));
        }

        [Test]
        public void GetServices_RegisteredType_ReturnsIEnumerable()
        {
            var testString = new List<object>{"Test string"};
            var type = typeof(string);
            _container.ResolveAll(type).Returns(testString);

            var resolvedString = _resolver.GetServices(type);

            _container.Received().ResolveAll(type);
            Assert.That(resolvedString, Is.EqualTo(testString));
        }

        [Test]
        public void GetServices_ExcludedTypes_ReturnsEmptyIEnumerable()
        {
            foreach (var excludedType in ExcludedTypes)
            {
                var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
                _container.ResolveAll(excludedType).Throws(exception);

                var resolvedObjects = _resolver.GetServices(excludedType);

                Assert.That(resolvedObjects, Is.Empty);
            }
        }

        [Test]
        public void GetServices_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var type = typeof(IDependencyResolver);
            var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
            _container.ResolveAll(type).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _resolver.GetServices(type));
        }

        [Test]
        public void BeginScope_CreatesResolverWithChildContainer()
        {
            var resolver = _resolver.BeginScope();

            _container.Received().CreateChildContainer();
            Assert.That(resolver, Is.Not.EqualTo(_resolver));
        }
    }
}
