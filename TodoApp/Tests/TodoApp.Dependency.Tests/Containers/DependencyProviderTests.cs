using System;
using System.Collections.Generic;
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
            var testString = "Test string";
            var type = testString.GetType();
            _unityContainer.Resolve(type).Returns(testString);

            var resolvedString = (string)_provider.Resolve(type);

            _unityContainer.Received().Resolve(type);
            Assert.That(resolvedString, Is.EqualTo(testString));
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
            var testStrings = new [] { "Test string" };
            var type = testStrings[0].GetType();
            _unityContainer.Resolve(type.MakeArrayType()).Returns(testStrings);

            var resolvedStrings = _provider.ResolveAll(type);

            Assert.That(resolvedStrings, Is.EquivalentTo(testStrings));
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
