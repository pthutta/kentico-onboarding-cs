using System;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TodoApp.Contracts.Containers;
using TodoApp.Dependency.Containers;
using Unity;
using Unity.Exceptions;

namespace TodoApp.Dependency.Tests.Containers
{
    [TestFixture]
    public class DependencyProviderTests
    {
        private IDependencyProvider _provider;
        private IUnityContainer _unityContainer;

        [SetUp]
        public void SetUp()
        {
            _unityContainer = Substitute.For<IUnityContainer>();
            _provider = new DependencyProvider(_unityContainer);
        }

        [Test]
        public void Resolve_RegisteredType_ReturnsObject()
        {
            var expectedInstance = new { value = "Test string" };
            var type = expectedInstance.GetType();
            _unityContainer.Resolve(type).Returns(expectedInstance);

            var actualInstance = _provider.Resolve(type);

            _unityContainer.Received(1).Resolve(type);
            Assert.That(actualInstance, Is.EqualTo(expectedInstance));
        }

        [Test]
        public void Resolve_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var type = typeof(string);
            var exception = new ResolutionFailedException(type, type.FullName, "Resolution failed", new Exception());
            _unityContainer.Resolve(type).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _provider.Resolve(type));
        }

        [Test]
        public void ResolveAll_RegisteredType_ReturnsObject()
        {
            var expectedInstances = new [] 
            {
                new { value = "Test string" },
                new { value = "Another string" },
                new { value = "This is the final one" }
            };
            var type = expectedInstances[0].GetType();
            _unityContainer.Resolve(type.MakeArrayType()).Returns(expectedInstances);

            var actualInstances = _provider.ResolveAll(type);

            Assert.That(actualInstances, Is.EquivalentTo(expectedInstances));
        }

        [Test]
        public void ResolveAll_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var type = typeof(string);
            var exception = new ResolutionFailedException(type, type.FullName, "Resolution failed", new Exception());
            _unityContainer.Resolve(type.MakeArrayType()).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _provider.ResolveAll(type));
        }

        [Test]
        public void CreateChildProvider_CreatesChildContainer()
        {
            var childProvider = _provider.CreateChildProvider();

            _unityContainer.Received().CreateChildContainer();
            Assert.That(childProvider, Is.Not.EqualTo(_provider));
        }
    }
}
