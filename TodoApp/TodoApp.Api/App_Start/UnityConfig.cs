using System.Web.Http;
using TodoApp.Api.Resolvers;
using TodoApp.Contracts.Repositories;
using TodoApp.DAL.Repositories;
using Unity;
using Unity.Lifetime;

namespace TodoApp.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IItemRepository, ItemRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}