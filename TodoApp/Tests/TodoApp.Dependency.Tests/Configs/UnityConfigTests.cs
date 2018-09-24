using System;
using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Dependency.Tests.Mocks.Containers;

namespace TodoApp.Dependency.Tests.Configs
{
    [TestFixture]
    public class UnityConfigTests
    {
        private static readonly Type[] ExcludedTypes = 
        {
            typeof(IBootstrap),
            typeof(IDependencyContainer)
        };

        private static readonly Type[] IncludedTypes =
        {
            typeof(HttpRequestMessage)
        };

        [Test]
        public void RegisterDependencies_RegistersRequiredDependencies()
        {
            var container = new TestContainer();
            var contractTypes = typeof(IBootstrap).Assembly.GetTypes();
            var types = contractTypes
                .Where(IsNotExcludedAndInterface)
                .Concat(IncludedTypes);

            DependencyConfig.RegisterDependencies(container);

            Assert.That(container.GetRegisteredTypes(), Is.EquivalentTo(types));
        }

        private static bool IsNotExcludedAndInterface(Type type)
            => type.IsInterface 
               && !Array.Exists(ExcludedTypes, excludedType => excludedType == type);
    }
}
