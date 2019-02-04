using System;
using System.Linq;
using System.Net.Http;
using NSubstitute;
using NUnit.Framework;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Routes;
using TodoApp.Dependency.Tests.Mocks.Containers;

namespace TodoApp.Dependency.Tests.Configs
{
    [TestFixture]
    public class UnityConfigTests
    {
        private DependencyConfig _dependencyConfig;

        private static readonly Type[] ExcludedTypes = 
        {
            typeof(IBootstrap),
            typeof(IDependencyContainer),
            typeof(IDependencyProvider)
        };

        private static readonly Type[] IncludedTypes =
        {
            typeof(HttpRequestMessage)
        };

        [SetUp]
        public void SetUp()
        {
            var routeNames = Substitute.For<IRouteNames>();
            _dependencyConfig = new DependencyConfig(routeNames);
        }

        [Test]
        public void RegisterDependencies_RegistersRequiredDependencies()
        {
            var contractTypes = typeof(IBootstrap).Assembly.GetTypes();
            var expectedTypes = contractTypes
                .Where(IsNotExcludedAndInterface)
                .Concat(IncludedTypes);
            var container = new TestContainer();

            _dependencyConfig.RegisterDependencies(container);
            var actualTypes = container.GetRegisteredTypes();

            Assert.That(actualTypes, Is.EquivalentTo(expectedTypes));
        }

        private static bool IsNotExcludedAndInterface(Type type)
            => type.IsInterface 
               && !Array.Exists(ExcludedTypes, excludedType => excludedType == type);
    }
}
