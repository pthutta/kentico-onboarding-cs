using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Enums;
using TodoApp.Contracts.Exceptions;
using TodoApp.Dependency.Containers;
using Unity;
using Unity.Exceptions;
using Unity.Injection;
using Unity.Lifetime;

namespace TodoApp.Dependency.Tests.Containers
{
    [TestFixture]
    public class DependencyContainerTests
    {
        private IDependencyContainer _container;
        private IUnityContainer _unityContainer;

        [SetUp]
        public void SetUp()
        {
            _unityContainer = Substitute.For<IUnityContainer>();
            _container = new DependencyContainer(_unityContainer);
        }

        [Test]
        public void RegisterType_SuccessfullyRegistersType()
        {
            var lifecycle = Lifecycle.SingletonPerApplication;

            _container.RegisterType<IDependencyContainer, DependencyContainer>(lifecycle);

            _unityContainer
                .Received()
                .RegisterType<IDependencyContainer, DependencyContainer>(
                    Arg.Any<ContainerControlledLifetimeManager>()
                );
        }

        [Test]
        public void RegisterType_RegistersTypeWithInstanceFactory()
        {
            var lifecycle = Lifecycle.SingletonPerRequest;
            var factory = Substitute.For<Func<DependencyContainer>>();

            _container.RegisterType(factory, lifecycle);

            _unityContainer
                .Received()
                .RegisterType<DependencyContainer>(
                    Arg.Any<HierarchicalLifetimeManager>(),
                    Arg.Any<InjectionFactory>()
                );
        }

        [Test]
        public void Resolve_RegisteredType_ReturnsObject()
        {
            var testString = "Test string";
            var type = testString.GetType();
            _unityContainer.Resolve(type).Returns(testString);

            var resolvedString = (string) _container.Resolve(type);

            _unityContainer.Received().Resolve(type);
            Assert.That(resolvedString, Is.EqualTo(testString));
        }

        [Test]
        public void Resolve_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var type = typeof(string);
            var exception = new ResolutionFailedException(type, type.FullName, "Resolution failed", new Exception());
            _unityContainer.Resolve(type).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _container.Resolve(type));
        }

        [Test]
        public void ResolveAll_RegisteredType_ReturnsObject()
        {
            var testStrings = new List<string> {"Test string"};
            var type = testStrings[0].GetType();
            _unityContainer.Resolve(type.MakeArrayType()).Returns(testStrings);

            var resolvedStrings = _container.ResolveAll(type);

            Assert.That(resolvedStrings, Is.EquivalentTo(testStrings));
        }

        [Test]
        public void ResolveAll_UnregisteredType_ThrowsDependencyResolutionFailedException()
        {
            var type = typeof(string);
            var exception = new ResolutionFailedException(type, type.FullName, "Resolution failed", new Exception());
            _unityContainer.Resolve(type.MakeArrayType()).Throws(exception);

            Assert.Throws<DependencyResolutionFailedException>(() => _container.ResolveAll(type));
        }

        [Test]
        public void CreateChildContainer_CreatesChildContainer()
        {
            var childContainer = _container.CreateChildContainer();

            _unityContainer.Received().CreateChildContainer();
            Assert.That(childContainer, Is.Not.EqualTo(_container));
        }
    }
}
