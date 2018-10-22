using System.Web.Http;
using System.Web.Http.Dependencies;
using TodoApp.ApiServices;
using TodoApp.Contracts.Containers;
using TodoApp.Contracts.Enums;
using TodoApp.Contracts.Routes;
using TodoApp.Database;
using TodoApp.Dependency.Containers;
using TodoApp.Dependency.Extensions;
using TodoApp.Dependency.Resolvers;
using TodoApp.Services;
using Unity;

namespace TodoApp.Dependency
{
    public class DependencyConfig
    {
        private readonly IRouteNames _routeNames;

        public DependencyConfig(IRouteNames routeNames)
            => _routeNames = routeNames;

        private static IDependencyContainer CreateGenericContainer()
        {
            var unityContainer = new UnityContainer();

            return new DependencyContainer(unityContainer);
        }

        internal void RegisterDependencies(IDependencyContainer container)
        {
            container
                .RegisterType(() => _routeNames, Lifecycle.SingletonPerRequest)
                .RegisterBootstrapper<DatabaseBootstrap>()
                .RegisterBootstrapper<ApiServicesBootstrap>()
                .RegisterBootstrapper<ServicesBootstrap>();
        }

        private static IDependencyResolver CreateResolver(IDependencyContainer container)
            => new DependencyResolver(container);


        public void Register(HttpConfiguration config)
        {
            var genericContainer = CreateGenericContainer();

            RegisterDependencies(genericContainer);

            config.DependencyResolver = CreateResolver(genericContainer);
        }
    }
}