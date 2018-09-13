using System.Web.Http;
using TodoApp.Api.Resolvers;
using TodoApp.Database.Configs;
using Unity;
using Unity.Lifetime;

namespace TodoApp.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            new DatabaseConfig().Register(container);
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}