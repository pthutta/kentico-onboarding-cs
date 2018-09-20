using System;
using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Containers;
using TodoApp.Dependency.Tests.Containers;

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

        [Test]
        public void RegisterDependencies_RegistersRequiredDependencies()
        {
            var container = new TestContainer();
            var contractTypes = typeof(IBootstrap).Assembly.GetTypes();
            var types = contractTypes
                .Where(IsNotExcludedInterface)
                .Append(typeof(HttpRequestMessage));

            DependencyConfig.RegisterDependencies(container);

            Assert.That(container.GetRegisteredTypes(), Is.EquivalentTo(types));
        }

        private static bool IsNotExcludedInterface(Type type)
            => type.IsInterface 
               && !Array.Exists(ExcludedTypes, excludedType => excludedType == type);
    }
}
