using System.Web.Http;
using System.Web.Http.Dependencies;
using TodoApp.ApiServices;
using TodoApp.Contracts.Containers;
using TodoApp.Database;
using TodoApp.Dependency.Containers;
using TodoApp.Dependency.Resolvers;
using Unity;

namespace TodoApp.Dependency
{
    public static class DependencyConfig
    {
        private static IDependencyContainer CreateGenericContainer()
        {
            var unityContainer = new UnityContainer();

            return new DependencyContainer(unityContainer);
        }

        internal static void RegisterDependencies(IDependencyContainer container)
        {
            container
                .RegisterBootstrapper<DatabaseBootstrap>()
                .RegisterBootstrapper<ApiServicesBootstrap>();
        }

        private static IDependencyResolver CreateResolver(IDependencyContainer container)
            => new DependencyResolver(container);


        public static void Register(HttpConfiguration config)
        {
            var genericContainer = CreateGenericContainer();

            RegisterDependencies(genericContainer);

            config.DependencyResolver = CreateResolver(genericContainer);
        }
    }
}