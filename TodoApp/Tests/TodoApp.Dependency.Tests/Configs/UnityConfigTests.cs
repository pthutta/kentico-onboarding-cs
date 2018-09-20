using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TodoApp.Dependency.Tests.Containers;

namespace TodoApp.Dependency.Tests.Configs
{
    [TestFixture]
    public class UnityConfigTests
    {
        private static readonly List<string> ExcludedNamespaces = new List<string>
        {
            "TodoApp.Contracts.Bootstraps",
            "TodoApp.Contracts.Containers"
        };

        [Test]
        public void RegisterDependencies_RegistersRequiredDependencies()
        {
            var container = new TestContainer();
            var interfaces = from t in Assembly.Load("TodoApp.Contracts").GetTypes()
                where t.IsInterface && !ExcludedNamespaces.Contains(t.Namespace)
                select t;

            DependencyConfig.RegisterDependencies(container);

            Assert.That(container.GetRegisteredTypes(), Is.SupersetOf(interfaces));
        }
    }
}
