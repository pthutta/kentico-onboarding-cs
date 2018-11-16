using System;
using System.Collections;
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
using TodoApp.Dependency.Resolvers;

namespace TodoApp.Dependency.Tests.Resolvers
{
    [TestFixture]
    public class DependencyResolverTests
    {
        private IDependencyResolver _resolver;
        private IDependencyProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = Substitute.For<IDependencyProvider>();
            _resolver = new DependencyResolver(_provider);
        }

        [Test]
        public void GetService_RegisteredType_ReturnsObject()
        {
            var expectedInstance = new { value = "Test string" };
            var resolutionType = expectedInstance.GetType();
            _provider.Resolve(resolutionType).Returns(expectedInstance);

            var actualInstance = _resolver.GetService(resolutionType);

            Assert.That(actualInstance, Is.EqualTo(expectedInstance));
        }

        [Test, TestCaseSource(typeof(DependencyResolverDataClass), nameof(DependencyResolverDataClass.TestCases))]
        public void GetService_ExcludedTypes_ReturnsNull(Type excludedType)
        {
            var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
            _provider.Resolve(excludedType).Throws(exception);

            var actualInstance = _resolver.GetService(excludedType);

            Assert.That(actualInstance, Is.Null);
        }

        [Test]
        public void GetService_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var resolutionType = typeof(IDependencyResolver);
            var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
            _provider.Resolve(resolutionType).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _resolver.GetService(resolutionType));
        }

        [Test]
        public void GetServices_RegisteredType_ReturnsIEnumerable()
        {
            var expectedInstances = new []
            {
                new { value = "Test string" },
                new { value = "Another string" },
                new { value = "This is the final one" }
            };
            var resolutionType = expectedInstances[0].GetType();
            _provider.ResolveAll(resolutionType).Returns(expectedInstances);

            var actualInstances = _resolver.GetServices(resolutionType);

            Assert.That(actualInstances, Is.EqualTo(expectedInstances));
        }

        [Test, TestCaseSource(typeof(DependencyResolverDataClass), nameof(DependencyResolverDataClass.TestCases))]
        public void GetServices_ExcludedTypes_ReturnsEmptyIEnumerable(Type excludedType)
        {
            var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
            _provider.ResolveAll(excludedType).Throws(exception);

            var actualInstance = _resolver.GetServices(excludedType);

            Assert.That(actualInstance, Is.Empty);
        }

        [Test]
        public void GetServices_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var resolutionType = typeof(IDependencyResolver);
            var exception = new DependencyResolutionFailedException("Resolution failed", new Exception());
            _provider.ResolveAll(resolutionType).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _resolver.GetServices(resolutionType));
        }

        [Test]
        public void BeginScope_CreatesResolverWithChildContainer()
        {
            var resolver = _resolver.BeginScope();

            _provider.Received().CreateChildProvider();
            Assert.That(resolver, Is.Not.EqualTo(_resolver));
        }
    }

    public class DependencyResolverDataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(typeof(IHostBufferPolicySelector));
                yield return new TestCaseData(typeof(IHttpControllerSelector));
                yield return new TestCaseData(typeof(IHttpControllerActivator));
                yield return new TestCaseData(typeof(IHttpActionSelector));
                yield return new TestCaseData(typeof(IHttpActionInvoker));
                yield return new TestCaseData(typeof(IContentNegotiator));
                yield return new TestCaseData(typeof(IExceptionHandler));
                yield return new TestCaseData(typeof(ModelMetadataProvider));
                yield return new TestCaseData(typeof(IModelValidatorCache));
                yield return new TestCaseData(typeof(ITraceWriter));
                yield return new TestCaseData(typeof(IReportApiVersions));
            }
        }
    }
}
