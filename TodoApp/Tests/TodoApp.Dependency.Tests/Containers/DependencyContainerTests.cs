using System;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Contracts.Containers;
using TodoApp.Dependency.Containers;
using Unity;
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
    }
}
