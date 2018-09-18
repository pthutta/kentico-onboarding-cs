using TodoApp.Contracts.Bootstraps;
using TodoApp.Contracts.Repositories;
using TodoApp.Database.Repositories;
using Unity;
using Unity.Lifetime;

namespace TodoApp.Database
{
    public class DatabaseBootstrap : IBootstrap
    {
        public IUnityContainer RegisterTypes(IUnityContainer container)
            => container.RegisterType<IItemRepository, ItemRepository>(new HierarchicalLifetimeManager());
    }
}
