using TodoApp.Contracts.Configs;
using TodoApp.Contracts.Repositories;
using TodoApp.Database.Repositories;
using Unity;
using Unity.Lifetime;

namespace TodoApp.Database
{
    public class DatabaseConfig : IConfig
    {
        public void Register(IUnityContainer container)
            => container.RegisterType<IItemRepository, ItemRepository>(new HierarchicalLifetimeManager());
    }
}
