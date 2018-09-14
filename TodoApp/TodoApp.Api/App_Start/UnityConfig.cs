using System.Web.Http;
using TodoApp.Api.Extensions;
using TodoApp.Api.Resolvers;
using TodoApp.Database.Configs;
using Unity;

namespace TodoApp.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .Register<DatabaseConfig>()
                .Register<UrlServiceConfig>();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}