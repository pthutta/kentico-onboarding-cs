using System.Web.Http;
using TodoApp.Api.Extensions;
using TodoApp.Api.Resolvers;
using TodoApp.Database;
using Unity;

namespace TodoApp.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .Register<DatabaseConfig>()
                .Register<ApiConfig>();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}