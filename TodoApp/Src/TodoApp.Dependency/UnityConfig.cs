using System.Web.Http.Dependencies;
using TodoApp.ApiServices;
using TodoApp.Database;
using TodoApp.Dependency.Containers;
using TodoApp.Dependency.Resolvers;
using Unity;

namespace TodoApp.Dependency
{
    public static class UnityConfig
    {
        public static IDependencyResolver GetDependencyResolver()
        {
            var container = new DependencyContainer(new UnityContainer())
                .RegisterBootstrapper<DatabaseBootstrap>()
                .RegisterBootstrapper<ApiServicesBootstrap>();
            return new UnityResolver(container);
        }
    }
}